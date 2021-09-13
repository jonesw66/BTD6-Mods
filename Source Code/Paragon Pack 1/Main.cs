using Assets.Main.Scenes;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Profile;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Mods;
using Assets.Scripts.Models.Towers.Upgrades;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Player;
using Assets.Scripts.Utils;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.ModOptions;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

[assembly: MelonInfo(typeof(Paragon_Pack_1.Main), "Paragon Pack 1", "1.0.0", "Weaboo Jones")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace Paragon_Pack_1
{
    public class Main : BloonsTD6Mod
    {
        static Dictionary<string, Type> paragons = new Dictionary<string, Type>()
        {
            { "BombShooter", typeof(BombParagon) },
            { "TackShooter", typeof(TackParagon) },
            { "IceMonkey", typeof(IceParagon) },
            { "GlueGunner", typeof(GlueParagon) },
            { "SniperMonkey", typeof(SniperParagon) }
        };

        static List<Tuple<TowerModel, UpgradeModel>> enabledParagons = new List<Tuple<TowerModel, UpgradeModel>>();

        static ModSettingBool bombParagon = new ModSettingBool(true)
        {
            displayName = "Bomb Paragon Enabled? (Requires restart)"
        };

        static ModSettingBool tackParagon = new ModSettingBool(true)
        {
            displayName = "Tack Paragon Enabled? (Requires restart)"
        };

        static ModSettingBool iceParagon = new ModSettingBool(true)
        {
            displayName = "Ice Paragon Enabled? (Requires restart)"
        };

        static ModSettingBool glueParagon = new ModSettingBool(true)
        {
            displayName = "Glue Paragon Enabled? (Requires restart)"
        };

        static ModSettingBool sniperParagon = new ModSettingBool(true)
        {
            displayName = "Sniper Paragon Enabled? (Requires restart)"
        };

        static List<ModSettingBool> settings = new List<ModSettingBool>()
        {
            bombParagon, tackParagon, iceParagon, glueParagon, sniperParagon
        };

        static ModSettingBool generateJSONs = new ModSettingBool(false)
        {
            displayName = "Generate JSONs",
            IsButton = true
        };

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();

            MelonLogger.Msg("Paragon Pack 1 Loaded!");
        }

        [HarmonyPatch(typeof(TitleScreen), "Start")]
        class TitleScreen_Start
        {
            [HarmonyPostfix]
            internal static void Postfix()
            {
                generateJSONs.OnInitialized.Add(option =>
                {
                    var buttonOption = (ButtonOption) option;
                    buttonOption.Button.AddOnClick(() =>
                    {
                        foreach (var pair in paragons)
                        {
                            FileIOUtil.SaveObject($"Upgrades\\{pair.Key} Paragon.json", (UpgradeModel)pair.Value.GetField("upgradeModel").GetValue(null));
                            FileIOUtil.SaveObject($"Towers\\{pair.Key}-Paragon.json", (TowerModel)pair.Value.GetField("towerModel").GetValue(null));
                        }

                        MelonLogger.Msg("Generated JSONs!");
                    });
                });

                foreach (var pair in paragons)
                {
                    foreach (var setting in settings)
                    {
                        if (setting.displayName.Contains(pair.Key.Substring(0, 2)) && setting)
                        {
                            enabledParagons.Add(new Tuple<TowerModel, UpgradeModel>(
                                (TowerModel)pair.Value.GetField("towerModel").GetValue(null),
                                (UpgradeModel) pair.Value.GetField("upgradeModel").GetValue(null)
                            ));

                            Game.instance.model.AddUpgrade(enabledParagons.Last().Item2);
                            Game.instance.model.AddTowerToGame(enabledParagons.Last().Item1);

                            MelonLogger.Msg(pair.Key + " Paragon Loaded!");
                            break;
                        }
                    }
                }

                Game.instance.GetLocalizationManager().textTable["BombShooter Paragon"] = "B.Y.O.B.";
                Game.instance.GetLocalizationManager().textTable["BombShooter Paragon Description"] = "Projectiles explode into 64+ Eliminator fragments that stun all but the strongest Bloons. Activate ability to unleash a blitzkrieg on MOABs!";

                Game.instance.GetLocalizationManager().textTable["TackShooter Paragon"] = "Industrial Revolution";
                Game.instance.GetLocalizationManager().textTable["TackShooter Paragon Description"] = "Huge tacks leave a trail of flames behind them, incinerating all bloons that come near. Upgraded ability transforms blades into fireballs!";

                Game.instance.GetLocalizationManager().textTable["IceMonkey Paragon"] = "'Til Hell Freezes Over";
                Game.instance.GetLocalizationManager().textTable["IceMonkey Paragon Description"] = "Subzero temperatures freeze bloons to the core, penetrating all layers. Crumble brittle bloons with the slightest touch!";

                Game.instance.GetLocalizationManager().textTable["GlueGunner Paragon"] = "Gorilla Glue";
                Game.instance.GetLocalizationManager().textTable["GlueGunner Paragon Description"] = "Stop Bloons in their tracks and disintegrate them on the spot. Super adhesive glue solution slows even the mighty BAD!";

                Game.instance.GetLocalizationManager().textTable["SniperMonkey Paragon"] = "S.E.A.L.";
                Game.instance.GetLocalizationManager().textTable["SniperMonkey Paragon Description"] = "Fully-automatic shots cripple Bloons on contact. S.E.A.L. is capable of bouncing bullets between Bloons across the map!";
            }
        }

        [HarmonyPatch(typeof(ProfileModel), nameof(ProfileModel.Validate))]
        class ProfileModel_Validate
        {
            [HarmonyPostfix]
            internal static void Postfix(ProfileModel __instance)
            {
                foreach (var paragonPair in enabledParagons)
                {
                    __instance.unlockedTowers.Add(paragonPair.Item1.name);

                    __instance.acquiredUpgrades.Add(paragonPair.Item2.name);
                }
            }
        }

        [HarmonyPatch(typeof(Btd6Player), nameof(Btd6Player.CheckShowParagonPip))]
        class Btd6Player_CheckShowParagonPip
        {
            [HarmonyPrefix]
            internal static bool Prefix(string towerId)
            {
                bool isCustomParagon = false;
                foreach (var paragonPair in enabledParagons)
                {
                    if (towerId == paragonPair.Item1.baseId)
                    {
                        isCustomParagon = true;
                    }
                }
                return !isCustomParagon;
            }
        }

        [HarmonyPatch(typeof(GameModel), nameof(GameModel.CreateModded), new Type[] { typeof(GameModel), typeof(Il2CppSystem.Collections.Generic.List<ModModel>) })]
        class GameModel_CreateModded
        {
            [HarmonyPostfix]
            internal static void Postfix(GameModel result)
            {
                foreach (var paragonPair in enabledParagons)
                {
                    string baseTower = paragonPair.Item1.baseId;

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
}
