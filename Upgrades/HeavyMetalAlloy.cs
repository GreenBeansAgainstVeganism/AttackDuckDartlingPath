using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Filters;
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
  class HeavyMetalAlloy : UpgradePlusPlus<DartlingPath>
  {
    public override int Cost => 14000;
    public override int Tier => 4;
    //public override string Icon => GetTextureGUID(Name + "-Icon");

    public override string Description => "A powerful non-newtonian substance engineered in monkey laboratories to deliver the maximum punch. Damages all bloon types, knocks back bigger blimps and also stuns smaller ones.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
      SlowForBloonModel slow = new SlowForBloonModel("SlowForBloonModel_",0.0f,0.25f,"Stun:Weak",9999999,"Stun",true,false,"Moab,Ddt","",false,null,true,false,false,0);


      ProjectileModel projectile = towerModel.GetAttackModel().weapons[0].projectile;

      projectile.pierce += 16f;
      if(towerModel.tiers[2] >= 2)
      {
        projectile.pierce += 8f;
      }

      if (towerModel.tiers[0] >= 2)
      {
        // boost laser shock damage
        projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = 2f;
        projectile.GetBehavior<DamageModifierForBloonStateModel>().damageAdditive = 2f;
      }

      DamageModel damagemodel = projectile.GetDamageModel();
      damagemodel.immuneBloonProperties &= ~BloonProperties.Lead;
      //damagemodel.damage += 1;

      KnockbackModel knockback = projectile.GetBehavior<KnockbackModel>();
      knockback.heavyMultiplier += 0.5f;
      knockback.lightMultiplier += 0.5f;

      PushBackModel pushback = projectile.GetBehavior<PushBackModel>();
      pushback.multiplierBFB = 0.4f;
      pushback.multiplierZOMG = 0.15f;

      projectile.GetBehavior<DamageModifierForTagModel>().damageAddative += 1;
      projectile.ApplyDisplay<Displays.Projectiles.GooballDisplay>();

      projectile.AddBehavior(slow);

      if (IsHighestUpgrade(towerModel))
      {
        towerModel.display = Game.instance.model.GetTowerFromId("DartlingGunner-003").display;
        if (!towerModel.GetAttackModel().HasBehavior<DisplayModel>())
        {
          towerModel.GetAttackModel().AddBehavior(Game.instance.model.GetTowerFromId("DartlingGunner-003").GetAttackModel().GetBehavior<DisplayModel>());
        }
        towerModel.GetAttackModel().GetBehavior<DisplayModel>().ApplyDisplay<Displays.Towers.HeavyMetalAlloyDisplay>();
      }

    }
  }
}