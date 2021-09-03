using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Extensions;

using HarmonyLib;
using MelonLoader;

using Assets.Main.Scenes;
using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Scripts.Models;
using Assets.Scripts.Models.TowerSets;
using Assets.Scripts.Models.Towers.Upgrades;
using Assets.Scripts.Unity;
using Assets.Scripts.Utils;
using Assets.Scripts.Models.Profile;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Simulation;
using Assets.Scripts.Models.Towers.Mods;
using Assets.Scripts.Unity.Player;

[assembly: MelonInfo(typeof(Glue_Paragon.Main), "Glue Paragon", "0.0.1", "Weaboo Jones")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace Glue_Paragon
{
    public class Main : BloonsTD6Mod
    {
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();

            MelonLogger.Msg("Glue Paragon Loaded!");
        }

        [HarmonyPatch(typeof(TitleScreen), "Start")]
        class TitleScreen_Start
        {
            [HarmonyPostfix]
            internal static void Postfix()
            {
                FileIOUtil.SaveObject("Upgrades\\GlueGunner Paragon.json", GlueParagon.upgradeModel);
                Game.instance.model.AddUpgrade(GlueParagon.upgradeModel);
                FileIOUtil.SaveObject("Towers\\GlueGunner-Paragon.json", GlueParagon.towerModel);
                Game.instance.model.AddTowerToGame(GlueParagon.towerModel);
            }
        }

        [HarmonyPatch(typeof(ProfileModel), nameof(ProfileModel.Validate))]
        class ProfileModel_Validate
        {
            [HarmonyPostfix]
            internal static void Postfix(ProfileModel __instance)
            {
                __instance.unlockedTowers.Add("GlueGunner-Paragon");

                __instance.acquiredUpgrades.Add("GlueGunner Paragon");
            }
        }

        [HarmonyPatch(typeof(Btd6Player), nameof(Btd6Player.CheckShowParagonPip))]
        class Btd6Player_CheckShowParagonPip
        {
            [HarmonyPrefix]
            internal static bool Prefix(string towerId)
            {
                return towerId != "GlueGunner";
            }
        }

        [HarmonyPatch(typeof(GameModel), nameof(GameModel.CreateModded), new Type[] { typeof(GameModel), typeof(Il2CppSystem.Collections.Generic.List<ModModel>) })]
        class GameModel_CreateModded
        {
            [HarmonyPostfix]
            internal static void Postfix(GameModel result)
            {
                for (int tier = 0; tier <= 2; tier++)
                {
                    result.GetTower("GlueGunner", 5, tier, 0).paragonUpgrade = new UpgradePathModel(upgrade: "GlueGunner Paragon", tower: "GlueGunner-Paragon");
                    result.GetTower("GlueGunner", 5, 0, tier).paragonUpgrade = new UpgradePathModel(upgrade: "GlueGunner Paragon", tower: "GlueGunner-Paragon");
                    result.GetTower("GlueGunner", tier, 5, 0).paragonUpgrade = new UpgradePathModel(upgrade: "GlueGunner Paragon", tower: "GlueGunner-Paragon");
                    result.GetTower("GlueGunner", 0, 5, tier).paragonUpgrade = new UpgradePathModel(upgrade: "GlueGunner Paragon", tower: "GlueGunner-Paragon");
                    result.GetTower("GlueGunner", tier, 0, 5).paragonUpgrade = new UpgradePathModel(upgrade: "GlueGunner Paragon", tower: "GlueGunner-Paragon");
                    result.GetTower("GlueGunner", 0, tier, 5).paragonUpgrade = new UpgradePathModel(upgrade: "GlueGunner Paragon", tower: "GlueGunner-Paragon");
                }
            }
        }
    }
}
