using System;
using System.Collections.Generic;
using System.Linq;
using TheEnchanter.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace TheEnchanter.Behaviors
{
    public class FortuneBlockBehavior : BlockBehavior
    {
        public FortuneBlockBehavior(Block block) : base(block)
        {
        }

        public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
        {
            var stack = byPlayer?.InventoryManager?.ActiveHotbarSlot?.Itemstack;

            List<ItemStack> toDrop = new();

            if (stack != null && stack.Attributes.HasAttribute("fortune")) //  Fortune handler
            {
                var level = stack.Attributes["fortune"].GetValue();
                if (!Validators.IsAValidLevel(level)) return null;
                float multiplier = Convert.ToSingle(level) * TheEnchanterModSystem.Config.Enchantments["fortune"].MultiplierByLevel;

                foreach (var drop in block.Drops)
                {
                    var stackDrop = drop.GetNextItemStack(multiplier);
                    if (stackDrop == null) continue;
                    toDrop.Add(stackDrop);
                }

                return toDrop.ToArray();
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
                if (block.BlockMaterial != EnumBlockMaterial.Ore) continue;

                block.BlockBehaviors = block.BlockBehaviors.Append(new FortuneBlockBehavior(block));
            }
        }
    }
}