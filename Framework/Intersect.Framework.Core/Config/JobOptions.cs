
using Intersect.Enums;

namespace Intersect.Framework.Core.Config
{
    /// <summary>
    /// Contains configurable options pertaining to the way Jobs are handled by the engine.
    /// </summary>
    public partial class JobOptions
    {
        /// <summary>
        /// The maximum level a job can achieve.
        /// </summary>
        public int MaxJobLevel { get; set; } = 100;

        /// <summary>
        /// Base experience required to level up jobs.
        /// </summary>
        public long BaseJobExp { get; set; } = 100;

        /// <summary>
        /// Growth rate for job experience required per level.
        /// </summary>
        public double ExpGrowthRate { get; set; } = 1.5;

        /// <summary>
        /// A dictionary containing base experience configurations for each job type.
        /// </summary>
        public Dictionary<JobType, long> JobBaseExp { get; set; } = new Dictionary<JobType, long>
            {
                { JobType.Farming, 100 },
                { JobType.Mining, 120 },
                { JobType.Fishing, 110 },
                { JobType.Lumberjack, 90 },
                { JobType.Cooking, 150 },
                { JobType.Alchemy, 130 },
                { JobType.Crafting, 140 },
                { JobType.Smithing, 160 },
                { JobType.Hunter, 170 }
            };

        /// <summary>
        /// Configures whether job progress notifications are shown to the player.
        /// </summary>
        public bool ShowJobProgressNotifications { get; set; } = true;

        /// <summary>
        /// Enables or disables automatic job leveling.
        /// </summary>
        public bool AutoJobLevelUp { get; set; } = true;

        /// <summary>
        /// Configures the maximum number of jobs a player can have active simultaneously.
        /// </summary>
        public int MaxActiveJobs { get; set; } = 3;

        /// <summary>
        /// Determines the rate at which job-related bonuses are applied (e.g., resource yield).
        /// </summary>
        public double JobBonusRate { get; set; } = 1.2;

        /// <summary>
        /// Enables or disables job-related penalties on death.
        /// </summary>
        public bool JobExpLossOnDeath { get; set; } = false;

        /// <summary>
        /// Percentage of job experience lost upon death if enabled.
        /// </summary>
        public int JobExpLossPercent { get; set; } = 10;
    }
    public enum JobType
    {
        //Recolection skills//
        None,
        Farming,
        Mining,
        Lumberjack,
        Fishing,
        Hunter,
        //Crafting skills//
        Cooking,
        Smithing,
        Alchemy,
        Crafting,
        JobCount
    }
}