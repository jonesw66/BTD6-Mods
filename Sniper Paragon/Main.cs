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

[assembly: MelonInfo(typeof(Sniper_Paragon.Main), "Sniper Paragon", "0.0.1", "Weaboo Jones")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace Sniper_Paragon
{
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();

            MelonLogger.Msg("Sniper Paragon Loaded!");
        }

        [HarmonyPatch(typeof(TitleScreen), "Start")]
        class TitleScreen_Start
        {
            [HarmonyPostfix]
            internal static void Postfix()
            {
                FileIOUtil.SaveObject("Upgrades\\SniperMonkey Paragon.json", SniperParagon.upgradeModel);
                Util.AddUpgrade(SniperParagon.upgradeModel);
                FileIOUtil.SaveObject("Towers\\SniperMonkey-Paragon.json", SniperParagon.towerModel);
                Util.AddTower(SniperParagon.towerModel);

                //Game.instance.GetLocalizationManager().textTable["SniperMonkey Paragon"] = "Sniper Paragon";
                //Game.instance.GetLocalizationManager().textTable["SniperMonkey Paragon Description"] = "INSANITY";
            }
        }

        [HarmonyPatch(typeof(ProfileModel), nameof(ProfileModel.Validate))]
        class ProfileModel_Validate
        {
            [HarmonyPostfix]
            internal static void Postfix(ProfileModel __instance)
            {
                __instance.unlockedTowers.Add("SniperMonkey-Paragon");

                __instance.acquiredUpgrades.Add("SniperMonkey Paragon");
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
                    result.GetTower("SniperMonkey", 5, tier, 0).paragonUpgrade = new UpgradePathModel(upgrade: "SniperMonkey Paragon", tower: "SniperMonkey-Paragon");
                    result.GetTower("SniperMonkey", 5, 0, tier).paragonUpgrade = new UpgradePathModel(upgrade: "SniperMonkey Paragon", tower: "SniperMonkey-Paragon");
                    result.GetTower("SniperMonkey", tier, 5, 0).paragonUpgrade = new UpgradePathModel(upgrade: "SniperMonkey Paragon", tower: "SniperMonkey-Paragon");
                    result.GetTower("SniperMonkey", 0, 5, tier).paragonUpgrade = new UpgradePathModel(upgrade: "SniperMonkey Paragon", tower: "SniperMonkey-Paragon");
                    result.GetTower("SniperMonkey", tier, 0, 5).paragonUpgrade = new UpgradePathModel(upgrade: "SniperMonkey Paragon", tower: "SniperMonkey-Paragon");
                    result.GetTower("SniperMonkey", 0, tier, 5).paragonUpgrade = new UpgradePathModel(upgrade: "SniperMonkey Paragon", tower: "SniperMonkey-Paragon");
                }
            }
        }
    }
}
