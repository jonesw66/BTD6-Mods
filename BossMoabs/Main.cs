using BTD_Mod_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using Assets.Scripts.Models;
using Assets.Scripts.Unity.UI_New.InGame;
using HarmonyLib;
using Assets.Scripts.Unity.Bridge;
using UnhollowerBaseLib;
using Assets.Scripts.Models.Rounds;
using BTD_Mod_Helper.Api.ModOptions;
using UnityEngine;

[assembly: MelonInfo(typeof(BossMoabs.Main), "Boss Moabs", "0.0.1", "Weaboo Jones")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace BossMoabs
{
    public class Main : BloonsTD6Mod
    {
        static ModSettingBool enableBossMoabs = new ModSettingBool(false)
        {
            displayName = "Treat Moab as Boss?"
        };

        public override void OnNewGameModel(GameModel result)
        {
            base.OnNewGameModel(result);

            foreach (var bloon in result.bloons)
            {
                MelonLogger.Msg(bloon.tags[0]);
                if (bloon.isMoab && enableBossMoabs)
                {
                    var withBossTag = new Il2CppStringArray(bloon.tags.Count + 1);
                    for (int i = 0; i < bloon.tags.Count; i++)
                    {
                        withBossTag[i] = bloon.tags[i];
                    }
                    withBossTag[bloon.tags.Count] = "Boss";

                    bloon.tags = withBossTag;
                }
            }
        }
    }
}
