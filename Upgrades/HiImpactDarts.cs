using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
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
  class HiImpactDarts : UpgradePlusPlus<DartlingPath>
  {
    public override int Cost => 1200;
    public override int Tier => 2;
    //public override string Icon => GetTextureGUID(Name + "-Icon");

    public override string DisplayName => "Hi-Impact Darts";
    public override string Description => "Bloons are knocked back a small amount when hit. (Buckshot gets increased knockback.)";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
      KnockbackModel knockback = Game.instance.model.GetTowerFromId("DartlingGunner-003").GetAttackModel().weapons[0].projectile.GetBehavior<KnockbackModel>().Duplicate();

      //knockback.moabMultiplier = 4f;
      knockback.heavyMultiplier = 1.1f;
      knockback.lightMultiplier = 1.5f;
      knockback.lifespan = 0.15f;

      foreach (ProjectileModel p in towerModel.GetDescendants<ProjectileModel>().ToArray())
      {
        if(p.HasBehavior<KnockbackModel>())
        {
          KnockbackModel m = p.GetBehavior<KnockbackModel>();
          //m.moabMultiplier += 0.5f;
          m.heavyMultiplier += 0.3f;
          m.lightMultiplier += 0.3f;
        }
        else
        {
          p.AddBehavior(knockback.Duplicate());
          p.collisionPasses = new int[2] { -1, 0 };
        }

        if(towerModel.tiers[0] >= 2)
        {
          p.collisionPasses = new int[3] { -1, 0, 1 };
        }
        
      }

    }
  }
}
