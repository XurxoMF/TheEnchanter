using System;
using System.Linq;
using TheEnchanter.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace TheEnchanter.Behaviors
{
    public class SilkTouchBlockBehavior : BlockBehavior
    {
        public SilkTouchBlockBehavior(Block block) : base(block)
        {
        }

        public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
        {
            var stack = byPlayer?.InventoryManager?.ActiveHotbarSlot?.Itemstack;

            if (stack?.Item?.Tool != null)
            {
                if (stack.Attributes.HasAttribute("silktouch")) // Silktouch handler
                {
                    handling = EnumHandling.PreventSubsequent;

                    var level = stack.Attributes["silktouch"].GetValue();
                    if (!Validators.IsAValidLevel(level)) return null;

                    if (Convert.ToInt32(level) >= 1) return new[] { new ItemStack(block) };
                }
            }

            return base.GetDrops(world, pos, byPlayer, ref dropChanceMultiplier, ref handling);
        }

        /// <summary>
        /// Add this Behavior to the specified Blocks.
        /// </summary>
        /// <param name="api">Api to access the block list.</param>
        public static void AddBehavior(ICoreAPI api)
        {
            foreach (var block in api.World.Blocks)
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new SilkTouchBlockBehavior(block));
            }
        }
    }
}