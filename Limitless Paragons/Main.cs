using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Simulation;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Simulation.Objects;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Simulation.Towers.Behaviors;
using Assets.Scripts.Simulation.Towers.Projectiles;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Bridge;
using Assets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using MelonLoader;
using UnhollowerBaseLib;
using static Assets.Scripts.Models.Towers.Behaviors.ParagonTowerModel;

[assembly: MelonInfo(typeof(Limitless_Paragons.Main), "Limitless Paragons", "0.0.1", "Weaboo Jones")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace Limitless_Paragons
{
    public class Main : BloonsTD6Mod
    {
        public static Dictionary<int, float> paragons = new Dictionary<int, float>();

        public static Dictionary<long, float> popsPerDegree = new Dictionary<long, float>();

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();

            MelonLogger.Msg("Limitless Paragons Loaded!");
        }

        public override void OnTitleScreen()
        {
            base.OnTitleScreen();

            CalculatePopsPerDegree(1, 1000);
        }

        static void CalculatePopsPerDegree(int minDegree, int maxDegree)
        {
            for (int degree = minDegree; degree <= maxDegree; degree++)
            {
                float requiredPops = degree > 200 ? 6575000f * degree - 864615000 : 12500f * degree * degree;
                popsPerDegree.Add(degree, requiredPops);
            }
        }

        static int UpdateDegree(int degree, float pops)
        {
            int nextDegree = degree;

            while (popsPerDegree[nextDegree] < pops)
            {
                MelonLogger.Msg("PopsPerDegree: " + popsPerDegree[nextDegree]);
                MelonLogger.Msg("Next Degree: " + nextDegree);
                nextDegree++;

                if (nextDegree > popsPerDegree.Count)
                {
                    CalculatePopsPerDegree(popsPerDegree.Count + 1, popsPerDegree.Count + 1000);
                }
            }
            return nextDegree;
        }

        static void UpdateDegreeDataModel(ref Tower tower, int degree)
        {
            var powerRequirements = InGame.instance.GetGameModel().paragonDegreeDataModel.powerDegreeRequirements;
            while (degree > powerRequirements.Count)
            {
                var newRequirements = new Il2CppStructArray<int>(powerRequirements.Count + 100);
                for (int power = 0; power < powerRequirements.Count + 100; power++)
                {
                    if (power < powerRequirements.Count)
                    {
                        newRequirements[power] = powerRequirements[power];
                    }
                    else
                    {
                        newRequirements[power] = newRequirements[power - 1] + 1;
                    }
                }
                powerRequirements = newRequirements;
            }
            InGame.instance.GetGameModel().paragonDegreeDataModel.powerDegreeRequirements = powerRequirements;

            ParagonTower.InvestmentInfo info = tower.GetTowerBehavior<ParagonTower>().investmentInfo;
            info.totalInvestment = Game.instance.model.paragonDegreeDataModel.powerDegreeRequirements[degree - 1];
            tower.GetTowerBehavior<ParagonTower>().investmentInfo = info;

            tower.GetTowerBehavior<ParagonTower>().UpdateDegree();
        }


        [HarmonyPatch(typeof(InGame), nameof(InGame.Update))]
        class InGame_Update
        {
            [HarmonyPostfix]
            internal static void Postfix()
            {
                if (InGame.instance?.bridge != null)
                {
                    var towers = InGame.instance.bridge.GetAllTowers();
                    for (int i = 0; i < towers.Count; i++)
                    {
                        var simTower = towers[i];
                        var tower = simTower.tower;
                        if (simTower.IsParagon)
                        {
                            if (!paragons.ContainsKey(tower.Id))
                            {
                                paragons.Add(tower.Id, tower.damageDealt);

                                var updatedDegree = UpdateDegree(tower.GetTowerBehavior<ParagonTower>().GetCurrentDegree(), tower.damageDealt);
                                UpdateDegreeDataModel(ref tower, updatedDegree);
                            }

                            if (tower.damageDealt > paragons[tower.Id])
                            {
                                paragons[tower.Id] = tower.damageDealt;

                                var degree = tower.GetTowerBehavior<ParagonTower>().GetCurrentDegree();
                                int nextDegree = UpdateDegree(degree, paragons[tower.Id]);
                                if (nextDegree > degree)
                                {
                                    UpdateDegreeDataModel(ref tower, nextDegree);
                                }
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(UnityToSimulation), nameof(UnityToSimulation.UpgradeTowerParagon))]
        class UnityToSimulation_UpgradeTowerParagon
        {
            [HarmonyPostfix]
            internal static void Postfix(int id)
            {
                foreach (var tower in InGame.instance.UnityToSimulation.GetAllTowers())
                {
                    if (tower.tower.Id == id)
                    {
                        paragons.Add(id, 0f);
                    }
                }
            }
        }

        public override void OnNewGameModel(GameModel result)
        {
            base.OnNewGameModel(result);

            result.paragonDegreeDataModel.attackCooldownReductionX = 70f;
            result.paragonDegreeDataModel.damagePercentPerDegree = 5.0f;
            result.paragonDegreeDataModel.damageIncreasePerDegree = 2.0f;
            result.paragonDegreeDataModel.damageIncreaseForDegrees = 30;
        }
    }
}
