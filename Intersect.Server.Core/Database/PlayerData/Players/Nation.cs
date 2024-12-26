using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Intersect.Enums;
using Intersect.Server.Entities;
using Intersect.Server.Networking;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Intersect.Network.Packets.Server;
using Intersect.GameObjects;
using Intersect.GameObjects.Maps;
using Intersect.Logging;
using Intersect.Utilities;
using Intersect.Server.Localization;
using Intersect.Server.Web.RestApi.Payloads;
using static Intersect.Server.Database.Logging.Entities.NationHistory;

namespace Intersect.Server.Database.PlayerData.Players
{
    /// <summary>
    /// A class containing the definition of each nation, alongside the methods to use them.
    /// </summary>
    public partial class Nation
    {

        public static ConcurrentDictionary<Guid, Nation> Nations = new ConcurrentDictionary<Guid, Nation>();

        // Entity Framework Garbage.
        public Nation()
        {
        }

        /// <summary>
        /// The database Id of the nations.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// The name of the nations.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The date on which this nations was founded.
        /// </summary>
        public DateTime FoundingDate { get; private set; }

        /// <summary>
        /// Contains a record of all nation members
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public ConcurrentDictionary<Guid, NationMember> Members { get; private set; } = new ConcurrentDictionary<Guid, NationMember>();

        /// <summary>
        /// The last time this nations status was updated and memberlist was send to online players
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public long LastUpdateTime { get; set; } = -1;

        /// <summary>
        /// Locking context to prevent saving this nation to the db twice at the same time, and to prevent bank items from being withdrawed/deposited into by 2 threads at the same time
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        private object mLock = new object();


        /// <summary>
        /// Create a new Nation instance.
        /// </summary>
        /// <param name="creator">The <see cref="Player"/> that created the nation.</param>
        /// <param name="name">The Name of the nation.</param>
        public static Nation CreateNation(Player creator, string name)
        {
            name = name.Trim();

            if (creator != null && FieldChecking.IsValidNationName(name, Strings.Regex.NationName))
            {
                using (var context = DbInterface.CreatePlayerContext(readOnly: false))
                {
                    var nation = new Nation()
                    {
                        Name = name,
                        FoundingDate = DateTime.UtcNow,
                    };

                    var player = context.Players.FirstOrDefault(p => p.Id == creator.Id);
                    if (player != null)
                    {
                        player.DbNation = nation;
                        player.NationJoinDate = DateTime.UtcNow;


                        context.ChangeTracker.DetectChanges();
                        context.SaveChanges();

                        var member = new NationMember(player.Id, player.Name, player.Level, player.ClassName, player.MapName);
                        nation.Members.AddOrUpdate(player.Id, member, (key, oldValue) => member);

                        creator.Nation = nation;
                        creator.NationJoinDate = DateTime.UtcNow;

                        // Send our entity data to nearby players.
                        PacketSender.SendEntityDataToProximity(Player.FindOnline(creator.Id));

                        Nations.AddOrUpdate(nation.Id, nation, (key, oldValue) => nation);

                        LogActivity(nation.Id, creator.Id, creator, NationActivityType.Created, name);

                        return nation;
                    }
                }
            }
            return null;

        }

        /// <summary>
        /// Loads a nation and it's members from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Nation LoadNation(Guid id)
        {
            if (!Nations.TryGetValue(id, out Nation found))
            {
                using var context = DbInterface.CreatePlayerContext();
                var nation = context.Nations.Where(g => g.Id == id).FirstOrDefault();
                if (nation == default)
                {
                    return default;
                }

                // Load Members
                var members = context.Players.Where(p => p.DbNation.Id == id).ToDictionary(t => t.Id, t => new Tuple<Guid, string, int, Guid, Guid>(t.Id, t.Name, t.Level, t.ClassId, t.MapId));
                foreach (var member in members)
                {
                    var nationMember = new NationMember(member.Value.Item1, member.Value.Item2, member.Value.Item3, ClassBase.GetName(member.Value.Item4), MapBase.GetName(member.Value.Item5));
                    nation.Members.AddOrUpdate(member.Key, nationMember, (key, oldValue) => nationMember);
                }

                Nations.AddOrUpdate(id, nation, (key, oldValue) => nation);

                return nation;
            }

            return found;
        }

        /// <summary>
        /// Find all online members of this nation.
        /// </summary>
        /// <returns>A list of online players.</returns>
        public List<Player> FindOnlineMembers()
        {
            var online = new List<Player>();
            foreach (var member in Members)
            {
                var plyr = Player.FindOnline(member.Key);
                if (plyr != null)
                {
                    //Update Cached Member List Values
                    member.Value.Name = plyr.Name;
                    member.Value.Class = plyr.ClassName;
                    member.Value.Level = plyr.Level;
                    member.Value.Map = plyr.MapName;

                    online.Add(plyr);
                }
            }

            return online;
        }

        /// <summary>
        /// Joins a player into the nation.
        /// </summary>
        /// <param name="player">The player to join into the nation.</param>
        public void AddMember(Player player)
        {
            if (player != null && !Members.Any(m => m.Key == player.Id))
            {
                using (var context = DbInterface.CreatePlayerContext(readOnly: false))
                {
                    var dbPlayer = context.Players.FirstOrDefault(p => p.Id == player.Id);
                    if (dbPlayer != null)
                    {
                        dbPlayer.DbNation = this;
                        dbPlayer.NationJoinDate = DateTime.UtcNow;
                        context.ChangeTracker.DetectChanges();
                        DetachNationFromDbContext(context, this);
                        context.SaveChanges();

                        player.Nation = this;
                        player.NationJoinDate = DateTime.UtcNow;

                        var member = new NationMember(player.Id, player.Name, player.Level, player.ClassName, player.MapName);
                        Members.AddOrUpdate(player.Id, member, (key, oldValue) => member);

                        // Send our new nation list to everyone that's online.
                        UpdateMemberList();

                        // Send our entity data to nearby players.
                        PacketSender.SendEntityDataToProximity(Player.FindOnline(player.Id));

                        LogActivity(Id, player.Id, player, NationActivityType.Joined);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            UpdateMemberList();
        }

        /// <summary>
        /// Send an updated version of our nation's memberlist to each online member.
        /// </summary>
        public void UpdateMemberList()
        {
            foreach (var member in FindOnlineMembers())
            {
                PacketSender.SendNation(member);
            }
            LastUpdateTime = Timing.Global.Milliseconds;
        }

        /// <summary>
        /// Check whether a specified player is a member of this nation.
        /// </summary>
        /// <param name="id">The Id to check against.</param>
        /// <returns>Whether or not this player is a member of this nation</returns>
        public bool IsMember(Guid id) => IsMember(Player.Find(id));

        /// <summary>
        /// Check whether a specified player is a member of this nation.
        /// </summary>
        /// <param name="player">The player to check against.</param>
        /// <returns>Whether or not this player is a member of this nation</returns>
        public bool IsMember(Player player)
        {
            return Members.ContainsKey(player?.Id ?? Guid.Empty);
        }

        /// <summary>
        /// Search for a nation by Id.
        /// </summary>
        /// <param name="id">The nation Id to search for.</param>
        /// <returns>Returns a <see cref="Nation"/> that matches the Id, if any.</returns>
        public static Nation GetNation(Guid id)
        {
            using (var context = DbInterface.CreatePlayerContext())
            {
                var nation = context.Nations.FirstOrDefault(g => g.Id == id);
                if (nation != null)
                {
                    return nation;
                }
            }

            return null;
        }

        /// <summary>
        /// Search for a nation by Name.
        /// </summary>
        /// <param name="name">The nation Name to search for.</param>
        /// <returns>Returns a <see cref="Nation"/> that matches the Name, if any.</returns>
        public static Nation GetNation(string name)
        {
            name = name.Trim();
            using (var context = DbInterface.CreatePlayerContext())
            {
                var nation = context.Nations.FirstOrDefault(g => g.Name.ToLower() == name.ToLower());
                if (nation != null)
                {
                    return nation;
                }
            }

            return null;
        }

        /// <summary>
        /// Determines wether or not a nation already exists with a given name
        /// </summary>
        /// <param name="name">Nation name to check</param>
        /// <returns></returns>
        public static bool NationExists(string name)
        {
            name = name.Trim().ToLower();
            using (var context = DbInterface.CreatePlayerContext())
            {
                return context.Nations.Any(g => g.Name.ToLower() == name.ToLower());
            }
        }

        /// <summary>
        /// Getter for the nation lock
        /// </summary>
        public object Lock => mLock;


        public static void DetachNationFromDbContext(PlayerContext context, Nation nation)
        {
            context.Entry(nation).State = EntityState.Detached;

        }

        /// <summary>
        /// Renames this nation and then saves the new name to the db
        /// </summary>
        /// <param name="name"></param>
        public bool Rename(string name)
        {
            if (NationExists(name) || !FieldChecking.IsValidNationName(name, Strings.Regex.NationName))
            {
                return false;
            }

            Name = name;
            foreach (var member in FindOnlineMembers())
            {
                PacketSender.SendEntityDataToProximity(member);
            }
            Save();

            return true;
        }


        /// <summary>
        /// Starts all common events with a specified trigger for all online nation members
        /// </summary>
        /// <param name="trigger">The common event trigger to run</param>
        /// <param name="command">The command which started this common event</param>
        /// <param name="param">Common event parameter</param>
        public void StartCommonEventsWithTriggerForAll(CommonEventTrigger trigger, string command, string param)
        {
            foreach (var plyr in FindOnlineMembers())
            {
                plyr.StartCommonEventsWithTrigger(trigger, command, param);
            }
        }


        /// <summary>
        /// Updates the db with this nation state
        /// </summary>
        public void Save()
        {
            lock (mLock)
            {
                using (var context = DbInterface.CreatePlayerContext(readOnly: false))
                {
                    context.Update(this);
                    context.ChangeTracker.DetectChanges();
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Retrieves a list of nations from the db as an IList
        /// </summary>
        public static IList<KeyValuePair<Nation, int>> List(string query, string sortBy, SortDirection sortDirection, int skip, int take, out int total)
        {
            try
            {
                using var context = DbInterface.CreatePlayerContext();
                var compiledQuery = string.IsNullOrWhiteSpace(query) ? context.Nations : context.Nations.Where(p => EF.Functions.Like(p.Name, $"%{query}%"));

                total = compiledQuery.Count();

                switch (sortBy?.ToLower() ?? "")
                {
                    case "members":
                        compiledQuery = sortDirection == SortDirection.Ascending ? compiledQuery.OrderBy(g => context.Players.Where(p => p.DbNation.Id == g.Id).Count()) : compiledQuery.OrderByDescending(g => context.Players.Where(p => p.DbNation.Id == g.Id).Count());
                        break;
                    case "foundingdate":
                        compiledQuery = sortDirection == SortDirection.Ascending ? compiledQuery.OrderBy(u => u.FoundingDate) : compiledQuery.OrderByDescending(u => u.FoundingDate);
                        break;
                    case "name":
                    default:
                        compiledQuery = sortDirection == SortDirection.Ascending ? compiledQuery.OrderBy(u => u.Name.ToUpper()) : compiledQuery.OrderByDescending(u => u.Name.ToUpper());
                        break;
                }

                return compiledQuery.Skip(skip).Take(take).Select(g => new KeyValuePair<Nation, int>(g, context.Players.Where(p => p.DbNation.Id == g.Id).Count())).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                total = 0;
                return null;
            }
        }
    }
}