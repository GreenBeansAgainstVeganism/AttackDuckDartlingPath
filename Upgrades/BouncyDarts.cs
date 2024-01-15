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
  class BouncyDarts : UpgradePlusPlus<DartlingPath>
  {
    public override int Cost => 210;
    public override int Tier => 1;
    //public override string Icon => GetTextureGUID(Name + "-Icon");

    public override string Description => "Allows projectiles to bounce off of solid obstacles.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
      ProjectileBlockerCollisionReboundModel bounce = Game.instance.model.GetTowerFromId("DartMonkey-300").GetAttackModel().weapons[0].projectile.GetBehavior<ProjectileBlockerCollisionReboundModel>().Duplicate();

      bounce.changeRotation = true;

      foreach (ProjectileModel p in towerModel.GetDescendants<ProjectileModel>().ToArray())
      {
        p.AddBehavior(bounce.Duplicate());
        
      };

      if(towerModel.tier <= 2)
      {
        towerModel.GetAttackModel().weapons[0].projectile.ApplyDisplay<Displays.Projectiles.BouncyDartDisplay>();
      }
    }
  }
}
