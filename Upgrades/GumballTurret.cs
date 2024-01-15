using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppSystem.IO;
using PathsPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackDuckDartlingPath.Upgrades
{
  class GumballTurret : UpgradePlusPlus<DartlingPath>
  {
    public override int Cost => 4800;
    public override int Tier => 3;
    //public override string Icon => GetTextureGUID(Name + "-Icon");

    public override string Description => "Shoots giant balls of goo at the bloons which hit more bloons, have increased lifespan, and can knock back small blimps.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
      //PushBackModel pushback = Game.instance.model.GetTowerFromId("BoomerangMonkey-004").GetAttackModel().GetDescendant<PushBackModel>().Duplicate();
      PushBackModel pushback = new PushBackModel("PushBackModel_", 3.0f, "Moabs", 1.0f, 0.15f, 1.0f, 0f, false);
      CollideExtraPierceReductionModel extrapierce = new CollideExtraPierceReductionModel("CollideExtraPierceModel_", "Moabs", 3, true);
      DamageModifierForTagModel moabdamage = new DamageModifierForTagModel("DamageModifierForTagModel_", "Moabs", 1.0f, 1.0f, false, false);
      //DamageModifierForTagModel moabdamage = Game.instance.model.GetTowerFromId("BombShooter-030").GetAttackModel().GetDescendant<DamageModifierForTagModel>();

      towerModel.GetAttackModel().weapons[0].rate *= 1.2f;
      if (towerModel.tiers[1] >= 2)
      {
        // reduce the effect of faster firing since it scales too well with the stun/knockback
        towerModel.GetAttackModel().weapons[0].rate *= 1.2f;
      }

      ProjectileModel projectile = towerModel.GetAttackModel().weapons[0].projectile;

      projectile.radius = 4.0f;
      projectile.pierce += 7f;
      projectile.GetBehavior<TravelStraitModel>().Lifespan *= 2.0f;
      if (towerModel.tiers[2] >= 2)
      {
        projectile.pierce += 2f;
        projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.5f;
      }

      projectile.GetDamageModel().immuneBloonProperties &= ~BloonProperties.Frozen;

      KnockbackModel knockback = projectile.GetBehavior<KnockbackModel>();
      knockback.heavyMultiplier += 0.4f;
      knockback.lightMultiplier += 0.5f;

      projectile.AddBehavior(pushback);
      projectile.AddBehavior(extrapierce);
      projectile.AddBehavior(moabdamage);
      projectile.hasDamageModifiers = true;
      projectile.ApplyDisplay<Displays.Projectiles.GumballDisplay>();

      if (IsHighestUpgrade(towerModel))
      {
        towerModel.display = Game.instance.model.GetTowerFromId("DartlingGunner-003").display;
        if (!towerModel.GetAttackModel().HasBehavior<DisplayModel>())
        {
          towerModel.GetAttackModel().AddBehavior(Game.instance.model.GetTowerFromId("DartlingGunner-003").GetAttackModel().GetBehavior<DisplayModel>());
        }
        towerModel.GetAttackModel().GetBehavior<DisplayModel>().ApplyDisplay<Displays.Towers.GumballTurretDisplay>();
      }
    }
  }
}