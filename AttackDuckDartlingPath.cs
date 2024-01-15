using MelonLoader;
using BTD_Mod_Helper;
using AttackDuckDartlingPath;
using PathsPlusPlus;
using Il2CppAssets.Scripts.Models.Towers;

[assembly: MelonInfo(typeof(AttackDuckDartlingPath.AttackDuckDartlingPath), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace AttackDuckDartlingPath
{
  public class AttackDuckDartlingPath : BloonsTD6Mod
  {
      public override void OnApplicationStart()
      {
          ModHelper.Msg<AttackDuckDartlingPath>("AttackDuckDartlingPath loaded!");
      }
  }

  public class DartlingPath : PathPlusPlus
  {
    public override string Tower => TowerType.DartlingGunner;

    public override int UpgradeCount => 5; // Increase this up to 5 as you create your Upgrades
  }
}
