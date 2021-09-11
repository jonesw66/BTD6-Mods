using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.ModOptions ;
using HarmonyLib;
using MelonLoader;
using UnhollowerBaseLib;

using Assets.Main.Scenes;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Bloons;
using Assets.Scripts.Simulation;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Simulation.Bloons.Behaviors;
using Assets.Scripts.Simulation.SimulationBehaviors;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.UI_New.InGame;

[assembly: MelonInfo(typeof(CashEqualizer.Main), "Cash Equalizer", "0.0.1", "Weaboo Jones")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace CashEqualizer
{
    public class Main : BloonsTD6Mod
    {
        static ModSettingBool customizeRoundMultipliers = new ModSettingBool(false)
        {
            displayName = "Customize Round Multipliers?"
        };
        static ModSettingDouble round1Multiplier = new ModSettingDouble(1f)
        {
            displayName = "Round 1-50 Multiplier: ",
            minValue = 0.01f,
            maxValue = 10f
        };
        static ModSettingDouble round51Multiplier = new ModSettingDouble(0.5f)
        {
            displayName = "Round 51-60 Multiplier: ",
            minValue = 0.01f,
            maxValue = 10f
        };
        static ModSettingDouble round61Multiplier = new ModSettingDouble(0.2f)
        {
            displayName = "Round 61-85 Multiplier: ",
            minValue = 0.01f,
            maxValue = 10f
        };
        static ModSettingDouble round86Multiplier = new ModSettingDouble(0.1f)
        {
            displayName = "Round 86-100 Multiplier: ",
            minValue = 0.01f,
            maxValue = 10f
        };
        static ModSettingDouble round101Multiplier = new ModSettingDouble(0.02f)
        {
            displayName = "Round 101+ Multiplier: ",
            minValue = 0.01f,
            maxValue = 10f
        };

        [HarmonyPatch(typeof(TitleScreen), "Start")]
        class TitleScreen_Start
        {
            [HarmonyPostfix]
            internal static void Postfix()
            {
                MelonLogger.Msg("Cash Equalizer Loaded!");
            }
        }

        [HarmonyPatch(typeof(Simulation), "AddCash")]
        public class Simulation_AddCash
        {
            [HarmonyPrefix]
            public static bool Prefix(ref double c, Simulation.CashType from, int cashIndex, ref Simulation.CashSource source)
            {
                float multi;
                int currentRound = InGame.instance.bridge.GetCurrentRound();

                if (currentRound < 50)         multi = 1 * (customizeRoundMultipliers ? (float) round1Multiplier : 1);
                else if (currentRound < 60)    multi = 2 * (customizeRoundMultipliers ? (float) round51Multiplier : 1);
                else if (currentRound < 85)    multi = 5 * (customizeRoundMultipliers ? (float) round61Multiplier : 1);
                else if (currentRound < 100)   multi = 10 * (customizeRoundMultipliers ? (float) round86Multiplier : 1);
                else                           multi = 50 * (customizeRoundMultipliers ? (float) round101Multiplier : 1);

                if (source == Simulation.CashSource.Normal && from != Simulation.CashType.EndOfRound)
                {
                    c *= multi;
                }

                return true;
            }
        }
    }
}
