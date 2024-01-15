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
  class SiegeEnginePrototype : UpgradePlusPlus<DartlingPath>
  {
    public override int Cost => 54000;
    public override int Tier => 5;
    //public override string Icon => GetTextureGUID(Name + "-Icon");
    //public override string DisplayName => "SIEGE Engine";

    public override string Description => "What happens when a wave of bloons meets an unstoppable force? Shoots gigantic wrecking balls which move slower but plow through bloons like they're not even there, knocking them back continuously as they roll and shattering reinforcements.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
      //PushBackModel pushback = Game.instance.model.GetTowerFromId("BoomerangMonkey-004").GetAttackModel().GetDescendant<PushBackModel>().Duplicate();
      //PushBackModel pushback = new PushBackModel("PushBackModel_", 3.0f, "Moabs", 1.0f, 0.175f, 1.0f, 0f, false);
      //CollideExtraPierceReductionModel extrapierce = new CollideExtraPierceReductionModel("CollideExtraPierceModel_", "Moabs", 3, true);
      //DamageModifierForTagModel moabdamage = new DamageModifierForTagModel("DamageModifierForTagModel_", "Moabs", 1.0f, 1.0f, false, false);
      //DamageModifierForTagModel moabdamage = Game.instance.model.GetTowerFromId("BombShooter-030").GetAttackModel().GetDescendant<DamageModifierForTagModel>();
      //SlowModel slow = new SlowModel("SlowModel_", 0.0f, 1.0f, "Stun:Weak", 9999999, "Stun", true, false, null, false, false, false, 0);
      //SlowModifierForTagModel moabslow = new SlowModifierForTagModel("SlowModifierForTagModel_moabStun", "Moab", "Stun:Weak", 1.0f, false, false, 0.25f, false);
      //SlowModifierForTagModel ddtslow = new SlowModifierForTagModel("SlowModifierForTagModel_ddtStun", "Ddt", "Stun:Weak", 1.0f, false, false, 0.25f, false);
      //SlowForBloonModel slow = new SlowForBloonModel("SlowForBloonModel_",0.0f,0.25f,"Stun:Weak",9999999,"Stun",true,false,"Moab,Ddt","",false,null,true,false,false,0);
      ClearHitBloonsModel rehit = new ClearHitBloonsModel("ClearHitBloonsModel_", 0.15f);
      Il2CppSystem.Collections.Generic.List<string> bloonTagExcludeList = new Il2CppSystem.Collections.Generic.List<string>(2);
      bloonTagExcludeList.Add("Zomg");
      bloonTagExcludeList.Add("Bad");
      RemoveBloonModifiersModel defortify = new RemoveBloonModifiersModel("RemoveBloonModifiersModel_", false, false, false, true, true, bloonTagExcludeList);


      ProjectileModel projectile = towerModel.GetAttackModel().weapons[0].projectile;

      towerModel.GetAttackModel().weapons[0].rate *= 5.0f;
      projectile.pierce += 336f;
      projectile.radius = 8.0f;
      
      if(towerModel.tiers[2] >= 2)
      {
        projectile.pierce += 168f;
      }

      if(towerModel.tiers[0] >= 2)
      {
        // boost laser shock damage
        projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = 10f;
        projectile.GetBehavior<DamageModifierForBloonStateModel>().damageAdditive = 10f;
      }

      DamageModel damagemodel = projectile.GetDamageModel();
      damagemodel.damage += 9;

      projectile.GetBehavior<TravelStraitModel>().Speed *= 0.5f;
      projectile.GetBehavior<TravelStraitModel>().Lifespan *= 2.0f;
      projectile.RemoveBehavior<DamageModifierForTagModel>();

      SlowForBloonModel slow = projectile.GetBehavior<SlowForBloonModel>();
      slow.bloonIds = new string[3] { "Moab", "Ddt", "Bfb" };
      slow.Lifespan = 0.5f;

      KnockbackModel knockback = projectile.GetBehavior<KnockbackModel>();
      knockback.heavyMultiplier += 1.0f;
      knockback.lightMultiplier += 1.0f;

      PushBackModel pushback = projectile.GetBehavior<PushBackModel>();
      pushback.multiplierZOMG = 0.25f;

      projectile.GetBehavior<ProjectileBlockerCollisionReboundModel>().changeRotation = false;
      projectile.GetBehavior<DisplayModel>().ignoreRotation = true;

      projectile.ApplyDisplay<Displays.Projectiles.CannonballDisplay>();

      projectile.AddBehavior(rehit);
      projectile.AddBehavior(defortify);

      if (IsHighestUpgrade(towerModel))
      {
        towerModel.display = Game.instance.model.GetTowerFromId("DartlingGunner-003").display;
        if (!towerModel.GetAttackModel().HasBehavior<DisplayModel>())
        {
          towerModel.GetAttackModel().AddBehavior(Game.instance.model.GetTowerFromId("DartlingGunner-003").GetAttackModel().GetBehavior<DisplayModel>());
        }
        towerModel.GetAttackModel().GetBehavior<DisplayModel>().ApplyDisplay<Displays.Towers.SiegeEnginePrototypeDisplay>();
        towerModel.displayScale = 1.3f;
      }
    }
  }
}