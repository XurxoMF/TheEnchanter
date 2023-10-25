using System;
using TheEnchanter.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace TheEnchanter.Behaviors
{
    public class SharpnessEntityBehavior : EntityBehavior
    {
        public SharpnessEntityBehavior(Entity entity) : base(entity)
        {
        }

        public override string PropertyName()
        {
            return "SharpnessEntityBehavior";
        }

        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);

            entity.GetBehavior<EntityBehaviorHealth>().onDamaged += OnShouldEntityReceiveDamage;
        }

        private float OnShouldEntityReceiveDamage(float dmg, DamageSource dmgSource)
        {
            entity.World.Api.Logger.Debug($"Dentro | {dmg} | {dmgSource?.SourceEntity?.GetType()} | {dmgSource?.SourceEntity is IPlayer}");

            if (dmgSource?.SourceEntity != null && dmgSource?.SourceEntity is EntityPlayer ePlayer)
            {
                IPlayer player = ePlayer?.Player;
                var stack = player?.InventoryManager?.ActiveHotbarSlot?.Itemstack;

                if (stack != null && stack.Attributes.HasAttribute("sharpness"))
                {
                    var level = stack.Attributes["sharpness"].GetValue();
                    if (!Validators.IsAValidLevel(level)) return dmg;
                    var multiplier = Convert.ToSingle(level) * TheEnchanterModSystem.Config.Enchantments["sharpness"].MultiplierByLevel + 1f;

                    entity.World.Api.Logger.Debug($"{dmg * multiplier}");
                    return dmg * multiplier;
                }
            }

            return dmg;
        }
    }
}