using HarmonyLib;
using TheEnchanter.Behaviors;
using TheEnchanter.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace TheEnchanter
{
    public class TheEnchanterModSystem : ModSystem
    {
        public static Harmony HarmonyInstance { get; set; } = new Harmony(Constants.ModID);

        public static Config Config { get; set; }

        public override bool AllowRuntimeReload => true;

        public override bool ShouldLoad(EnumAppSide side)
        {
            return true;
        }

        public override void StartPre(ICoreAPI api)
        {
            base.StartPre(api);

            Config = ConfigManager.ReadConfig(api);
        }

        public override void Start(ICoreAPI api)
        {
            base.Start(api);

            // Register all the BlockBehaviors
            api.RegisterBlockBehaviorClass("FortuneBlockBehavior", typeof(FortuneBlockBehavior));
            api.RegisterBlockBehaviorClass("SilkTouchBlockBehavior", typeof(SilkTouchBlockBehavior));
            api.RegisterEntityBehaviorClass("SharpnessEntityBehavior", typeof(SharpnessEntityBehavior));
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            base.StartServerSide(api);

            // Apply all the Harmony patches.
            HarmonyInstance.PatchAll();
        }

        public override void Dispose()
        {
            HarmonyInstance.UnpatchAll();

            base.Dispose();
        }

        public override void AssetsFinalize(ICoreAPI api)
        {
            base.AssetsFinalize(api);

            // Add Behaviors to all the Blocks, filtered in each function
            FortuneBlockBehavior.AddBehavior(api);
            SilkTouchBlockBehavior.AddBehavior(api);
            // SharpnessEntityBehavior added with assets.
        }
    }
}
