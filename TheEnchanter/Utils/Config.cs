using System.Collections.Generic;
using Vintagestory.API.Common;

namespace TheEnchanter.Utils
{
    /// <summary>
    /// Config for this mod.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Dictionary<"name", Enchantment> with all the available enchantments.
        /// </summary>
        public Dictionary<string, Enchantment> Enchantments = new()
        {
            { "fortune", DeffaultConfigs.Fortune },
            { "silktouch", DeffaultConfigs.Silktouch },
            { "unbreaking", DeffaultConfigs.Unbreaking }
        };

        /// <summary>
        /// Deffault constructor for the LoadModConfig method.
        /// </summary>
        public Config()
        {
        }
    }

    /// <summary>
    /// Class for Enchantemts config.
    /// </summary>
    public class Enchantment
    {
        /// <summary>
        /// If this enchantemt will be enabled.
        /// </summary>
        public bool Enabled = true;
        /// <summary>
        /// Max level this enchantment can get.
        /// </summary>
        public int MaxLevel = 1;
        /// <summary>
        /// In enchantments with per-level scaling it's the multiplier for each level.<br/>0.1f will be 10% at level 1, 20% at level 2, 30% at level 3...<br/>Must be 0 if the enchantment don't have an scaling effect (like silktouch).
        /// </summary>
        public float MultiplierByLevel = 0f;

        /// <summary>
        /// Deffault constructor for the LoadModConfig method.
        /// </summary>
        public Enchantment()
        {
        }

        /// <summary>
        /// Constructor to create an Enchantent manually.
        /// </summary>
        /// <param name="Enabled">If this enchantment will be enabled.</param>
        /// <param name="MaxLevel">Max level this enchantment can get.</param>
        /// <param name="MultiplierByLevel">Multiplier for each enchantment level, set to 0 to enchantment with no scaling effect (like silktouch).</param>
        public Enchantment(bool Enabled, int MaxLevel, float MultiplierByLevel = 0f)
        {
            this.Enabled = Enabled;
            this.MaxLevel = MaxLevel;
            this.MultiplierByLevel = MultiplierByLevel;
        }
    }

    /// <summary>
    /// Deffault configs for each enchantment.
    /// </summary>
    public static class DeffaultConfigs
    {
        /// <summary>
        /// Fortune deffault config.
        /// </summary>
        public static readonly Enchantment Fortune = new(true, 3, 0.1f);
        /// <summary>
        /// Silktouch deffault config.
        /// </summary>
        public static readonly Enchantment Silktouch = new(true, 1, 0f);
        /// <summary>
        /// Unbreaking deffault config.
        /// </summary>
        public static readonly Enchantment Unbreaking = new(true, 3, 0.1f);
    }

    public static class ConfigManager
    {
        /// <summary>
        /// Load, validate and save the config file.
        /// </summary>
        /// <param name="api"></param>
        public static Config ReadConfig(ICoreAPI api)
        {
            Config config;

            // Try loading config.
            try
            {
                config = LoadConfig(api);

                if (config == null)
                {
                    GenerateConfig(api);
                    config = LoadConfig(api);
                }
                else
                {
                    // Validators in case some value is wrong.
                    FortuneValidator(api, ref config);
                    SilktouchValidator(api, ref config);
                    UnbreakingValidator(api, ref config);

                    GenerateConfig(api, config);
                }
            }
            catch
            {
                api.Logger.Error($"[{Constants.ModName}] There is no config file or it contain errors! Applying default config!");
                GenerateConfig(api);
                config = LoadConfig(api);
            }

            api.Logger.Event($"[{Constants.ModName}] Config succesfully loaded.");

            return config;
        }

        private static Config LoadConfig(ICoreAPI api)
        {
            return api.LoadModConfig<Config>($"{Constants.ModName}.json");
        }

        private static void GenerateConfig(ICoreAPI api)
        {
            api.StoreModConfig(new Config(), $"{Constants.ModName}.json");
        }

        private static void GenerateConfig(ICoreAPI api, Config previousConfig)
        {
            api.StoreModConfig(previousConfig, $"{Constants.ModName}.json");
        }

        /// <summary>
        /// Validates the Enchantments["fortune"] config.
        /// </summary>
        /// <param name="api">ICoreAPI</param>
        /// <param name="config">ref Config to modify the values.</param>
        private static void FortuneValidator(ICoreAPI api, ref Config config)
        {
            if (!config.Enchantments.ContainsKey("fortune")) config.Enchantments.Add("fortune", DeffaultConfigs.Fortune);

            if (config.Enchantments["fortune"] == null) config.Enchantments["fortune"] = DeffaultConfigs.Fortune;

            // MaxLevel
            if (config.Enchantments["fortune"].MaxLevel < 1) config.Enchantments["fortune"].MaxLevel = DeffaultConfigs.Fortune.MaxLevel;
            // MultiplierByLevel
            if (config.Enchantments["fortune"].MultiplierByLevel < 0f) config.Enchantments["fortune"].MultiplierByLevel = DeffaultConfigs.Fortune.MultiplierByLevel;
        }

        /// <summary>
        /// Validates the Enchantments["silktouch"] config.
        /// </summary>
        /// <param name="api">ICoreAPI</param>
        /// <param name="config">ref Config to modify the values.</param>
        private static void SilktouchValidator(ICoreAPI api, ref Config config)
        {
            if (!config.Enchantments.ContainsKey("silktouch")) config.Enchantments.Add("silktouch", DeffaultConfigs.Silktouch);

            if (config.Enchantments["silktouch"] == null) config.Enchantments["silktouch"] = DeffaultConfigs.Silktouch;

            // MaxLevel
            if (config.Enchantments["silktouch"].MaxLevel < 1) config.Enchantments["silktouch"].MaxLevel = DeffaultConfigs.Silktouch.MaxLevel;
            // MultiplierByLevel
            if (config.Enchantments["silktouch"].MultiplierByLevel != 0f) config.Enchantments["silktouch"].MultiplierByLevel = DeffaultConfigs.Silktouch.MultiplierByLevel;
        }

        /// <summary>
        /// Validates the Enchantments["unbreaking"] config.
        /// </summary>
        /// <param name="api">ICoreAPI</param>
        /// <param name="config">ref Config to modify the values.</param>
        private static void UnbreakingValidator(ICoreAPI api, ref Config config)
        {
            if (!config.Enchantments.ContainsKey("unbreaking")) config.Enchantments.Add("unbreaking", DeffaultConfigs.Unbreaking);

            if (config.Enchantments["unbreaking"] == null) config.Enchantments["unbreaking"] = DeffaultConfigs.Unbreaking;

            // MaxLevel
            if (config.Enchantments["unbreaking"].MaxLevel < 1) config.Enchantments["unbreaking"].MaxLevel = DeffaultConfigs.Unbreaking.MaxLevel;
            // MultiplierByLevel
            if (config.Enchantments["unbreaking"].MultiplierByLevel < 0f) config.Enchantments["unbreaking"].MultiplierByLevel = DeffaultConfigs.Unbreaking.MultiplierByLevel;
        }
    }
}