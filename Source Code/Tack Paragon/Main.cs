using Assets.Main.Scenes;
using Assets.Scripts.Models;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Profile;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.Towers.Mods;
using Assets.Scripts.Models.Towers.Upgrades;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Player;
using Assets.Scripts.Utils;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: MelonInfo(typeof(Tack_Paragon.Main), "Tack Paragon", "0.1.0", "Weaboo Jones")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace Tack_Paragon
{
    public class Main : BloonsTD6Mod
    {
        public static string baseTower = "TackShooter";

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();

            MelonLogger.Msg("Tack Paragon Loaded!");
        }

        [HarmonyPatch(typeof(TitleScreen), "Start")]
        class TitleScreen_Start
        {
            [HarmonyPostfix]
            internal static void Postfix()
            {
                FileIOUtil.SaveObject($"Upgrades\\{baseTower}Paragon.json", Paragon.upgradeModel);
                Game.instance.model.AddUpgrade(Paragon.upgradeModel);
                FileIOUtil.SaveObject($"Towers\\{baseTower}-Paragon.json", Paragon.towerModel);
                Game.instance.model.AddTowerToGame(Paragon.towerModel);

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
                __instance.unlockedTowers.Add($"{baseTower}-Paragon");

                __instance.acquiredUpgrades.Add($"{baseTower} Paragon");
            }
        }

        [HarmonyPatch(typeof(Btd6Player), nameof(Btd6Player.CheckShowParagonPip))]
        class Btd6Player_CheckShowParagonPip
        {
            [HarmonyPrefix]
            internal static bool Prefix(string towerId)
            {
                return towerId != baseTower;
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
                    result.GetTower($"{baseTower}", 5, tier, 0).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                    result.GetTower($"{baseTower}", 5, 0, tier).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                    result.GetTower($"{baseTower}", tier, 5, 0).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                    result.GetTower($"{baseTower}", 0, 5, tier).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                    result.GetTower($"{baseTower}", tier, 0, 5).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                    result.GetTower($"{baseTower}", 0, tier, 5).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                }
            }
        }
    }
}
