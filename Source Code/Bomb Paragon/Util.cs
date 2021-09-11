using Assets.Scripts.Models;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Upgrades;
using Assets.Scripts.Models.Towers.Weapons;
using Assets.Scripts.Unity;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;

namespace Bomb_Paragon
{
    public static class Util
    {
        public static void AddTower(TowerModel towerModel)
        {
            Game.instance.model.towers = Game.instance.model.towers.AddTo(towerModel);
            Game.instance.model.AddChildDependant(towerModel);
        }

        public static void AddUpgrade(UpgradeModel upgradeModel)
        {
            Game.instance.model.upgrades = Game.instance.model.upgrades.AddTo(upgradeModel);
            Game.instance.model.upgradesByName.Add(upgradeModel.name, upgradeModel);
            Game.instance.model.AddChildDependant(upgradeModel);
        }

        public static Il2CppReferenceArray<T> AddTo<T>(this Il2CppReferenceArray<T> referenceArray, T objectToAdd) where T : Il2CppSystem.Object
        {
            if (referenceArray is null)
                referenceArray = new Il2CppReferenceArray<T>(0);

            Il2CppReferenceArray<T> newRef = new Il2CppReferenceArray<T>(referenceArray.Count + 1);
            for (int i = 0; i < referenceArray.Count; i++)
                newRef[i] = referenceArray[i];

            newRef[newRef.Length - 1] = objectToAdd;

            return newRef;
        }

        public static T Duplicate<T>(this T model) where T : Model
        {
            return model.Clone().Cast<T>();
        }

        public static void AddBehavior<T>(this TowerModel model, T behavior) where T : Model
        {
            model.behaviors = model.behaviors.AddTo(behavior);
            model.AddChildDependant(behavior);
        }

        public static void AddBehavior<T>(this ProjectileModel model, T behavior) where T : Model
        {
            model.behaviors = model.behaviors.AddTo(behavior);
            model.AddChildDependant(behavior);
        }

        public static bool IsType<T>(this Il2CppSystem.Object instance) where T : Il2CppSystem.Object
        {
            return instance?.TryCast<T>() != null;
        }

        public static T Find<T>(this Il2CppReferenceArray<Model> behaviors) where T : Model
        {
            foreach (var behavior in behaviors)
            {
                if (typeof(T) == typeof(WeaponModel))
                {
                    MelonLogger.Msg(behavior?.TryCast<T>() != null);
                }
                if (behavior?.TryCast<T>() != null)
                {
                    return behavior.Cast<T>();
                }
            }
            return null;
        }

        public static Il2CppReferenceArray<T> FindAll<T>(this Il2CppReferenceArray<Model> behaviors) where T : Model
        {
            var output = new Il2CppReferenceArray<T>(0);

            foreach (var behavior in behaviors)
            {
                if (behavior?.TryCast<T>() != null)
                {
                    output = output.AddTo(behavior.Cast<T>());
                }
            }
            return output;
        }

        public static AttackModel GetAttackModel(this TowerModel model)
        {
            return model.GetBehavior<AttackModel>();
        }

        public static WeaponModel GetWeapon(this TowerModel model)
        {
            return model.GetAttackModel().weapons[0];
        }

        public static T GetBehavior<T>(this TowerModel model) where T : Model
        {
            return model.behaviors.Find<T>().Cast<T>();
        }

        public static T GetBehavior<T>(this AttackModel model) where T : Model
        {
            return model.behaviors.Find<T>().Cast<T>();
        }

        public static T GetBehavior<T>(this ProjectileModel model) where T : Model
        {
            return model.behaviors.Find<T>().Cast<T>();
        }
        
        public static Il2CppReferenceArray<T> GetBehaviors<T>(this TowerModel model) where T : Model
        {
            return model.behaviors.FindAll<T>();
        }

        public static Il2CppReferenceArray<T> GetBehaviors<T>(this ProjectileModel model) where T : Model
        {
            return model.behaviors.FindAll<T>();
        }
    }
}
