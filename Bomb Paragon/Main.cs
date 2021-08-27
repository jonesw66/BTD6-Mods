using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
using Assets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu;
using Assets.Scripts.Unity.Towers;
using UnhollowerBaseLib;
using System.Reflection;
using Assets.Scripts.Unity.Player;

[assembly: MelonInfo(typeof(Bomb_Paragon.Main), "Bomb Paragon", "0.0.1", "Weaboo Jones")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace Bomb_Paragon
{
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();

            MelonLogger.Msg("Bomb Paragon Loaded!");
        }

        [HarmonyPatch(typeof(TitleScreen), "Start")]
        class TitleScreen_Start
        {
            [HarmonyPostfix]
            internal static void Postfix()
            {
                FileIOUtil.SaveObject("Upgrades\\BombShooter Paragon.json", BombParagon.upgradeModel);
                Util.AddUpgrade(BombParagon.upgradeModel);

                FileIOUtil.SaveObject("Towers\\BombShooter-Paragon.json", BombParagon.towerModel);
                Util.AddTower(BombParagon.towerModel);

                //Game.instance.GetLocalizationManager().textTable["BombShooter Paragon"] = "Bomb Paragon";
                //Game.instance.GetLocalizationManager().textTable["BombShooter Paragon Description"] = "INSANITY";
            }
        }

        [HarmonyPatch(typeof(ProfileModel), nameof(ProfileModel.Validate))]
        class ProfileModel_Validate
        {
            [HarmonyPostfix]
            internal static void Postfix(ProfileModel __instance)
            {
                __instance.unlockedTowers.Add("BombShooter-Paragon");

                __instance.acquiredUpgrades.Add("BombShooter Paragon");
            }
        }

        [HarmonyPatch(typeof(Btd6Player), nameof(Btd6Player.CheckShowParagonPip))]
        class Btd6Player_CheckShowParagonPip
        {
            [HarmonyPrefix]
            internal static bool Prefix(string towerId)
            {
                return towerId != "BombShooter";
            }
        }

        [HarmonyPatch(typeof(GameModel))]
        [HarmonyPatch(nameof(GameModel.CreateModded))]
        [HarmonyPatch(new Type[] { typeof(GameModel), typeof(Il2CppSystem.Collections.Generic.List<ModModel>) })]
        class GameModel_CreateModded
        {
            [HarmonyPostfix]
            internal static void Postfix(GameModel result)
            {
                for (int tier = 0; tier <= 2; tier++)
                {
                    result.GetTower("BombShooter", 5, tier, 0).paragonUpgrade = new UpgradePathModel(upgrade: "BombShooter Paragon", tower: "BombShooter-Paragon");
                    result.GetTower("BombShooter", 5, 0, tier).paragonUpgrade = new UpgradePathModel(upgrade: "BombShooter Paragon", tower: "BombShooter-Paragon");
                    result.GetTower("BombShooter", tier, 5, 0).paragonUpgrade = new UpgradePathModel(upgrade: "BombShooter Paragon", tower: "BombShooter-Paragon");
                    result.GetTower("BombShooter", 0, 5, tier).paragonUpgrade = new UpgradePathModel(upgrade: "BombShooter Paragon", tower: "BombShooter-Paragon");
                    result.GetTower("BombShooter", tier, 0, 5).paragonUpgrade = new UpgradePathModel(upgrade: "BombShooter Paragon", tower: "BombShooter-Paragon");
                    result.GetTower("BombShooter", 0, tier, 5).paragonUpgrade = new UpgradePathModel(upgrade: "BombShooter Paragon", tower: "BombShooter-Paragon");
                }
            }
        }
    }
}
