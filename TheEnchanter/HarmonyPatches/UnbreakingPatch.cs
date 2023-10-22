using System;
using HarmonyLib;
using TheEnchanter.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;

namespace TheEnchanter.HarmonyPatches
{
    [HarmonyPatch(typeof(CollectibleObject), nameof(CollectibleObject.DamageItem))]
    public static class CollectibleObjectDamageItemPatch
    {
        public static void Prefix(CollectibleObject __instance, IWorldAccessor world, Entity byEntity, ItemSlot itemslot, ref int amount)
        {
            var stack = itemslot?.Itemstack;

            if (stack.Attributes.HasAttribute("unbreaking"))
            {
                var level = stack.Attributes["unbreaking"].GetValue();
                if (!Validators.IsAValidLevel(level)) return;
                float multiplier = Convert.ToSingle(level) * TheEnchanterModSystem.Config.Enchantments["unbreaking"].MultiplierByLevel;
                float random = new Random().NextSingle();

                if (random <= multiplier)
                {
                    amount = 0;
                }
            }
        }
    }

    [HarmonyPatch(typeof(ItemWearable), nameof(ItemWearable.DamageItem))]
    public static class ItemWearableDamageItemPatch
    {
        public static void Prefix(CollectibleObject __instance, IWorldAccessor world, Entity byEntity, ItemSlot itemslot, ref int amount)
        {
            var stack = itemslot?.Itemstack;

            if (stack.Attributes.HasAttribute("unbreaking"))
            {
                var level = stack.Attributes["unbreaking"].GetValue();
                if (!Validators.IsAValidLevel(level)) return;
                float multiplier = Convert.ToSingle(level) * TheEnchanterModSystem.Config.Enchantments["unbreaking"].MultiplierByLevel;
                float random = new Random().NextSingle();

                if (random <= multiplier)
                {
                    amount = 0;
                }
            }
        }
    }
}