﻿// <auto-generated />
using System;
using Intersect.Server.Database.PlayerData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Intersect.Server.Migrations.Sqlite.Player
{
    [DbContext(typeof(SqlitePlayerContext))]
    partial class SqlitePlayerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Api.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT")
                        .HasColumnOrder(0);

                    b.Property<Guid>("ClientId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Issued")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Ticket")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Ban", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Banner")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Ip")
                        .HasColumnType("TEXT");

                    b.Property<string>("Reason")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT")
                        .HasColumnName("PlayerId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Bans");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Mute", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Ip")
                        .HasColumnType("TEXT");

                    b.Property<string>("Muter")
                        .HasColumnType("TEXT");

                    b.Property<string>("Reason")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT")
                        .HasColumnName("PlayerId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Mutes");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.Bag", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("SlotCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Bags");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.BagSlot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("BagId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ItemPropertiesJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("ItemProperties");

                    b.Property<Guid>("ParentBagId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Slot")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BagId");

                    b.HasIndex("ParentBagId");

                    b.ToTable("Bag_Items");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.BankSlot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("BagId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ItemPropertiesJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("ItemProperties");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Slot")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BagId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Player_Bank");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.Friend", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("OwnerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("TargetId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("TargetId");

                    b.ToTable("Player_Friends");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.Guild", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<byte>("BackgroundB")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("BackgroundG")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("BackgroundR")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BankSlotsCount")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Experience")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("FoundingDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GuildInstanceId")
                        .HasColumnType("TEXT");

                    b.Property<int>("GuildPoints")
                        .HasColumnType("INTEGER");

                    b.Property<string>("GuildUpgradesData")
                        .HasColumnType("TEXT");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LogoBackground")
                        .HasColumnType("TEXT");

                    b.Property<string>("LogoSymbol")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("SpentGuildPoints")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("SymbolB")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("SymbolG")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SymbolPosY")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("SymbolR")
                        .HasColumnType("INTEGER");

                    b.Property<float>("SymbolScale")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.GuildBankSlot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("BagId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GuildId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ItemPropertiesJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("ItemProperties");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Slot")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BagId");

                    b.HasIndex("GuildId");

                    b.ToTable("Guild_Bank");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.GuildVariable", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GuildId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Json")
                        .HasColumnType("TEXT")
                        .HasColumnName("Value");

                    b.Property<Guid>("VariableId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("VariableId", "GuildId")
                        .IsUnique();

                    b.ToTable("Guild_Variables");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.HotbarSlot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("BagId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ItemOrSpellId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Slot")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StatBuffsJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("PreferredStatBuffs");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Player_Hotbar");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.InventorySlot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("BagId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ItemPropertiesJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("ItemProperties");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Slot")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BagId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Player_Items");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.MailBox", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("AttachmentsJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("Attachments");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("SenderId");

                    b.ToTable("Player_MailBox");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.PlayerVariable", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Json")
                        .HasColumnType("TEXT")
                        .HasColumnName("Value");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("VariableId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("VariableId", "PlayerId")
                        .IsUnique();

                    b.ToTable("Player_Variables");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.Quest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Completed")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("QuestId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("TEXT");

                    b.Property<int>("TaskProgress")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("QuestId", "PlayerId")
                        .IsUnique();

                    b.ToTable("Player_Quests");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.SpellSlot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Slot")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("SpellId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Player_Spells");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.UserVariable", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Json")
                        .HasColumnType("TEXT")
                        .HasColumnName("Value");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("VariableId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VariableId", "UserId")
                        .IsUnique();

                    b.ToTable("User_Variables");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT")
                        .HasColumnOrder(0);

                    b.Property<string>("Email")
                        .HasColumnType("TEXT")
                        .HasColumnOrder(2);

                    b.Property<string>("LastIp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasColumnOrder(1);

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordResetCode")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PasswordResetTime")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("PlayTimeSeconds")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PowerJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("Power");

                    b.Property<DateTime?>("RegistrationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Salt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Intersect.Server.Entities.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT")
                        .HasColumnOrder(0);

                    b.Property<Guid>("ClassId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("DbGuildId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Dir")
                        .HasColumnType("INTEGER");

                    b.Property<long>("DonateXPGuild")
                        .HasColumnType("INTEGER");

                    b.Property<string>("EquipmentJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("Equipment");

                    b.Property<long>("Exp")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Face")
                        .HasColumnType("TEXT");

                    b.Property<string>("FooterLabelJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("FooterLabel");

                    b.Property<int>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<float>("GuildExpPercentage")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("GuildJoinDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("GuildRank")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HeaderLabelJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("HeaderLabel");

                    b.Property<int>("InstanceType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ItemCooldownsJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("ItemCooldowns");

                    b.Property<string>("JobsJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("JobsData");

                    b.Property<string>("JsonColor")
                        .HasColumnType("TEXT")
                        .HasColumnName("Color");

                    b.Property<DateTime?>("LastOnline")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("LastOverworldMapId")
                        .HasColumnType("TEXT")
                        .HasColumnName("LastOverworldMapId");

                    b.Property<int>("LastOverworldX")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LastOverworldY")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("MapId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasColumnOrder(1);

                    b.Property<string>("NameColorJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("NameColor");

                    b.Property<Guid>("PersonalMapInstanceId")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("PlayTimeSeconds")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SharedInstanceRespawnDir")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("SharedInstanceRespawnId")
                        .HasColumnType("TEXT")
                        .HasColumnName("SharedInstanceRespawnId");

                    b.Property<int>("SharedInstanceRespawnX")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SharedInstanceRespawnY")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("SharedMapInstanceId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SpellCooldownsJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("SpellCooldowns");

                    b.Property<string>("Sprite")
                        .HasColumnType("TEXT");

                    b.Property<int>("StatPoints")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StatPointsJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("StatPointAllocations");

                    b.Property<string>("StatsJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("BaseStats");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("VitalsJson")
                        .HasColumnType("TEXT")
                        .HasColumnName("Vitals");

                    b.Property<int>("X")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Y")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Z")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DbGuildId");

                    b.HasIndex("UserId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Api.RefreshToken", b =>
                {
                    b.HasOne("Intersect.Server.Database.PlayerData.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Ban", b =>
                {
                    b.HasOne("Intersect.Server.Database.PlayerData.User", "User")
                        .WithOne("Ban")
                        .HasForeignKey("Intersect.Server.Database.PlayerData.Ban", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Mute", b =>
                {
                    b.HasOne("Intersect.Server.Database.PlayerData.User", "User")
                        .WithOne("Mute")
                        .HasForeignKey("Intersect.Server.Database.PlayerData.Mute", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.BagSlot", b =>
                {
                    b.HasOne("Intersect.Server.Database.PlayerData.Players.Bag", "Bag")
                        .WithMany()
                        .HasForeignKey("BagId");

                    b.HasOne("Intersect.Server.Database.PlayerData.Players.Bag", "ParentBag")
                        .WithMany("Slots")
                        .HasForeignKey("ParentBagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bag");

                    b.Navigation("ParentBag");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.BankSlot", b =>
                {
                    b.HasOne("Intersect.Server.Database.PlayerData.Players.Bag", "Bag")
                        .WithMany()
                        .HasForeignKey("BagId");

                    b.HasOne("Intersect.Server.Entities.Player", "Player")
                        .WithMany("Bank")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bag");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.Friend", b =>
                {
                    b.HasOne("Intersect.Server.Entities.Player", "Owner")
                        .WithMany("Friends")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Intersect.Server.Entities.Player", "Target")
                        .WithMany()
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Owner");

                    b.Navigation("Target");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.GuildBankSlot", b =>
                {
                    b.HasOne("Intersect.Server.Database.PlayerData.Players.Bag", "Bag")
                        .WithMany()
                        .HasForeignKey("BagId");

                    b.HasOne("Intersect.Server.Database.PlayerData.Players.Guild", "Guild")
                        .WithMany("Bank")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bag");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.GuildVariable", b =>
                {
                    b.HasOne("Intersect.Server.Database.PlayerData.Players.Guild", "Guild")
                        .WithMany("Variables")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.HotbarSlot", b =>
                {
                    b.HasOne("Intersect.Server.Entities.Player", "Player")
                        .WithMany("Hotbar")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.InventorySlot", b =>
                {
                    b.HasOne("Intersect.Server.Database.PlayerData.Players.Bag", "Bag")
                        .WithMany()
                        .HasForeignKey("BagId");

                    b.HasOne("Intersect.Server.Entities.Player", "Player")
                        .WithMany("Items")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bag");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.MailBox", b =>
                {
                    b.HasOne("Intersect.Server.Entities.Player", "Player")
                        .WithMany("MailBoxs")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Intersect.Server.Entities.Player", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Player");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.PlayerVariable", b =>
                {
                    b.HasOne("Intersect.Server.Entities.Player", "Player")
                        .WithMany("Variables")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.Quest", b =>
                {
                    b.HasOne("Intersect.Server.Entities.Player", "Player")
                        .WithMany("Quests")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.SpellSlot", b =>
                {
                    b.HasOne("Intersect.Server.Entities.Player", "Player")
                        .WithMany("Spells")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.UserVariable", b =>
                {
                    b.HasOne("Intersect.Server.Database.PlayerData.User", "User")
                        .WithMany("Variables")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Intersect.Server.Entities.Player", b =>
                {
                    b.HasOne("Intersect.Server.Database.PlayerData.Players.Guild", "DbGuild")
                        .WithMany()
                        .HasForeignKey("DbGuildId");

                    b.HasOne("Intersect.Server.Database.PlayerData.User", "User")
                        .WithMany("Players")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DbGuild");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.Bag", b =>
                {
                    b.Navigation("Slots");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.Players.Guild", b =>
                {
                    b.Navigation("Bank");

                    b.Navigation("Variables");
                });

            modelBuilder.Entity("Intersect.Server.Database.PlayerData.User", b =>
                {
                    b.Navigation("Ban");

                    b.Navigation("Mute");

                    b.Navigation("Players");

                    b.Navigation("RefreshTokens");

                    b.Navigation("Variables");
                });

            modelBuilder.Entity("Intersect.Server.Entities.Player", b =>
                {
                    b.Navigation("Bank");

                    b.Navigation("Friends");

                    b.Navigation("Hotbar");

                    b.Navigation("Items");

                    b.Navigation("MailBoxs");

                    b.Navigation("Quests");

                    b.Navigation("Spells");

                    b.Navigation("Variables");
                });
#pragma warning restore 612, 618
        }
    }
}
