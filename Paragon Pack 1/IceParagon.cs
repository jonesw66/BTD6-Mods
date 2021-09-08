using Assets.Scripts.Models;
using Assets.Scripts.Models.Bloons.Behaviors;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.Towers.Mods;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Upgrades;
using Assets.Scripts.Models.Towers.Weapons;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Simulation.SMath;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Utils;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnityEngine;

namespace Paragon_Pack_1
{
    static class IceParagon
    {
        public static TowerModel towerModel;
        public static UpgradeModel upgradeModel;

        public static string baseTower = "IceMonkey";

        static IceParagon()
        {
            CreateUpgrade();

            InitializeTower();

            AddGenericBehaviors();

            AddCommonBehaviors();

            CombineUniqueBehaviors();

            CustomizeTower();
        }

        static void CreateUpgrade()
        {
            upgradeModel = new UpgradeModel(
                name: $"{baseTower} Paragon",
                cost: 500000,
                xpCost: 0,
                icon: new SpriteReference(guid: "d6b7340621dee7d4ab2a48c0cbd8b529"), // Icicle Impale
                path: -1,
                tier: 5,
                locked: 0,
                confirmation: "Paragon",
                localizedNameOverride: ""
            );
        }

        static void InitializeTower()
        {
            var model = Game.instance.model.GetTower(baseTower).Duplicate();

            towerModel = new TowerModel();

            towerModel.name = $"{baseTower}-Paragon";
            towerModel.display = model.display;
            towerModel.baseId = baseTower;

            towerModel.cost = 500000f;
            towerModel.towerSet = "Primary";
            towerModel.radius = 6f;
            towerModel.radiusSquared = 36f;
            towerModel.range = 55f;

            towerModel.ignoreBlockers = false;
            towerModel.isGlobalRange = false;
            towerModel.areaTypes = model.areaTypes;

            towerModel.tier = 6;
            towerModel.tiers = Game.instance.model.GetTowerFromId($"DartMonkey-Paragon").tiers;

            towerModel.icon = model.icon;
            towerModel.portrait = model.portrait;
            towerModel.instaIcon = model.instaIcon;

            towerModel.ignoreTowerForSelection = false;
            towerModel.footprint = model.footprint.Duplicate();
            towerModel.dontDisplayUpgrades = false;
            towerModel.emoteSpriteSmall = null;
            towerModel.emoteSpriteLarge = null;
            towerModel.doesntRotate = false;

            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(0);
            var appliedUpgrades = new Il2CppStringArray(6);
            for (int upgrade = 0; upgrade < 5; upgrade++)
            {
                appliedUpgrades[upgrade] = Game.instance.model.GetTowerFromId($"{baseTower}-500").appliedUpgrades[upgrade];
            }
            appliedUpgrades[5] = $"{baseTower} Paragon";
            towerModel.appliedUpgrades = appliedUpgrades;

            towerModel.paragonUpgrade = null;
            towerModel.isSubTower = false;
            towerModel.isBakable = true;
            towerModel.powerName = null;
            towerModel.showPowerTowerBuffs = false;
            towerModel.animationSpeed = 1f;
            towerModel.towerSelectionMenuThemeId = "Default";
            towerModel.ignoreCoopAreas = false;
            towerModel.canAlwaysBeSold = false;
            towerModel.isParagon = true;
        }

        static void AddGenericBehaviors()
        {
            var model500 = Game.instance.model.GetTowerFromId($"{baseTower}-500").Duplicate();

            towerModel.mods = new Il2CppReferenceArray<ApplyModModel>(0);
            towerModel.mods = towerModel.mods.AddTo(model500.mods[1]);
            towerModel.mods = towerModel.mods.AddTo(model500.mods[2]);
            towerModel.mods = towerModel.mods.AddTo(model500.mods[3]);
            towerModel.mods = towerModel.mods.AddTo(model500.mods[4]);

            towerModel.AddBehavior(model500.GetBehavior<CreateEffectOnPlaceModel>());
            towerModel.AddBehavior(model500.GetBehavior<CreateSoundOnTowerPlaceModel>());
            towerModel.AddBehavior(model500.GetBehavior<CreateSoundOnUpgradeModel>());
            towerModel.AddBehavior(model500.GetBehavior<CreateSoundOnSellModel>());
            towerModel.AddBehavior(model500.GetBehavior<CreateEffectOnSellModel>());
            towerModel.AddBehavior(model500.GetBehavior<CreateEffectOnUpgradeModel>());
            towerModel.AddBehavior(model500.GetBehavior<DisplayModel>());
        }

        static void AddCommonBehaviors()
        {
            var model500 = Game.instance.model.GetTowerFromId($"{baseTower}-500").Duplicate();

            towerModel.mods = towerModel.mods.AddTo(model500.mods[5]);

            //var attackBehaviors = new Il2CppReferenceArray<Model>(0);

            //var weaponBehaviors = new Il2CppReferenceArray<WeaponBehaviorModel>(0);
            //weaponBehaviors = weaponBehaviors.AddTo(model500.GetWeapon().GetBehavior<CreateSoundOnProjectileCreatedModel>());

            //var commonWeapon = new WeaponModel(
            //    name: $"{baseTower}_Common_WeaponModel",
            //    animation: 1,
            //    animationOffset: 0.1f,
            //    fireWithoutTarget: false,
            //    fireBetweenRounds: false,
            //    emission:new SingleEmissionModel(name: "SingleEmission", behaviors: null),
            //    behaviors: weaponBehaviors,
            //    useAttackPosition: false,
            //    startInCooldown: false,
            //    customStartCooldown: 0f,
            //    animateOnMainAttack: false
            //);

            //towerModel.AddBehavior(new AttackModel(
            //    name: $"{baseTower}_AttackModel",
            //    weapons: new Il2CppReferenceArray<WeaponModel>(new WeaponModel[] { commonWeapon }),
            //    range: 50f,
            //    behaviors: attackBehaviors,
            //    targetProvider: null,
            //    offsetX: 0f,
            //    offsetY: 0f,
            //    offsetZ: 0f,
            //    attackThroughWalls: false,
            //    fireWithoutTarget: false,
            //    framesBeforeRetarget: 0,
            //    addsToSharedGrid: true,
            //    sharedGridRange: 0f
            //));
        }

        static void CombineUniqueBehaviors()
        {
            var model500 = Game.instance.model.GetTowerFromId($"{baseTower}-500").Duplicate();
            var model050 = Game.instance.model.GetTowerFromId($"{baseTower}-050").Duplicate();
            var model005 = Game.instance.model.GetTowerFromId($"{baseTower}-005").Duplicate();

            towerModel.mods = towerModel.mods.AddTo(model500.mods[0]);
            towerModel.mods = towerModel.mods.AddTo(model050.mods[0]);
            towerModel.mods = towerModel.mods.AddTo(model005.mods[0]);
            towerModel.targetTypes = model005.targetTypes;

            var freezeWaterModel = model050.GetBehavior<FreezeNearbyWaterModel>();
            freezeWaterModel.radius = 999f;
            towerModel.AddBehavior(freezeWaterModel);

            towerModel.AddBehavior(model005.GetAttackModel());
            towerModel.AddBehavior(model500.GetAttackModel());
            towerModel.AddBehavior(model050.GetBehavior<SlowBloonsZoneModel>());
            towerModel.AddBehavior(model050.GetAbility());
        }

        static void CustomizeTower()
        {
            var model050 = Game.instance.model.GetTowerFromId($"{baseTower}-050").Duplicate();
            var model005 = Game.instance.model.GetTowerFromId($"{baseTower}-005").Duplicate();
            var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();

            towerModel.display = model005.display;
            towerModel.AddBehavior(model005.GetBehavior<DisplayModel>());

            towerModel.ApplyDisplay<Wind>();
            towerModel.AddBehavior(boomerangParagon.GetBehavior<ParagonTowerModel>());
            towerModel.GetBehavior<ParagonTowerModel>().displayDegreePaths.ForEach(path => path.assetPath = ModDisplay.GetInstance<Wind>().Id);
            towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());

            towerModel.GetBehavior<SlowBloonsZoneModel>().zoneRadius = 68f;
            towerModel.GetBehavior<SlowBloonsZoneModel>().radiusOffset = 0;
            towerModel.GetBehavior<SlowBloonsZoneModel>().speedScale = 0.4f;
            towerModel.GetBehavior<SlowBloonsZoneModel>().bindRadiusToTowerRange = false;
            towerModel.GetBehavior<SlowBloonsZoneModel>().filters = new Il2CppReferenceArray<FilterModel>(0);

            var slowZone = new SlowBloonsZoneModel(
                name: "Slow",
                zoneRadius: 80f,
                mutationId: "WindSlow",
                isUnique: true,
                filters: null,
                speedScale: 0.8f,
                speedChange: 0,
                bindRadiusToTowerRange: false,
                radiusOffset: 0,
                bloonTag: "Moabs",
                inclusive: true
            );
            towerModel.AddBehavior(slowZone);
            towerModel.AddBehavior(new LinkProjectileRadiusToTowerRangeModel(name: "AOE", projectileModel: towerModel.GetAttackModels()[1].weapons[0].projectile, baseTowerRange: 50f, projectileRadiusOffset: 0, displayRadius: 20f));

            towerModel.GetAttackModels()[0].range = 55f;
            towerModel.GetAttackModels()[0].weapons[0].Rate = 0.5f;
            towerModel.GetAttackModels()[0].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = 50f;
            towerModel.GetAttackModels()[0].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.radius = 15f;
            towerModel.GetAttackModels()[0].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModel>().damage = 5f;
            towerModel.GetAttackModels()[0].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModel>().immuneBloonProperties = 0;
            towerModel.GetAttackModels()[0].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<FreezeModel>().layers = 999;
            towerModel.GetAttackModels()[0].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<FreezeModel>().Lifespan = 5f;
            towerModel.GetAttackModels()[0].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<FreezeModel>().damageModel = towerModel.GetAttackModels()[0].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModel>();
            towerModel.GetAttackModels()[0].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<CarryProjectileModel>().projectile.pierce = 5f;
            towerModel.GetAttackModels()[0].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<CarryProjectileModel>().projectile.GetBehavior<DamageModel>().damage = 5f;
            towerModel.GetAttackModels()[0].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModifierForTagModel>().damageAddative = 75f;

            towerModel.GetAttackModels()[1].range = 50f;
            towerModel.GetAttackModels()[1].weapons[0].Rate = 1f;
            towerModel.GetAttackModels()[1].weapons[0].GetBehavior<EjectEffectModel>().effectModel.scale = 8.5f;
            towerModel.GetAttackModels()[1].weapons[0].projectile.pierce = 60f;
            towerModel.GetAttackModels()[1].weapons[0].projectile.radius = 50f;
            //towerModel.GetAttackModels()[1].weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<EmitOnPopModel>().emission = new RandomArcEmissionModel(name: "Random", count: 1, offset: 0, angle: 360, randomAngle: 360, startOffset: 0, behaviors: null);
            var emitOnDestroy = new EmitOnDestroyModel(
                name: "DestroyEmit",
                projectile: towerModel.GetAttackModels()[0].weapons[0].projectile,
                emission: towerModel.GetAttackModels()[1].weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<EmitOnPopModel>().emission,
                tower: 0
            );
            towerModel.GetAttackModels()[1].weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().AddBehavior(emitOnDestroy);
            towerModel.GetAttackModels()[1].weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().RemoveBehavior<EmitOnPopModel>();
            towerModel.GetAttackModels()[1].weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().layers = 1;
            towerModel.GetAttackModels()[1].weapons[0].projectile.GetBehaviors<SlowModel>()[0].layers = 999;
            towerModel.GetAttackModels()[1].weapons[0].projectile.GetBehaviors<SlowModel>()[0].Multiplier = 0.25f;
            towerModel.GetAttackModels()[1].weapons[0].projectile.GetBehaviors<FreezeModel>()[0].layers = 999;
            towerModel.GetAttackModels()[1].weapons[0].projectile.GetBehaviors<FreezeModel>()[0].Lifespan = 7f;
            towerModel.GetAttackModels()[1].weapons[0].projectile.GetBehaviors<SlowModel>()[1].layers = 999;
            towerModel.GetAttackModels()[1].weapons[0].projectile.GetBehaviors<SlowModel>()[1].Multiplier = 0.6f;

            towerModel.GetAttackModels()[1].RemoveBehavior<AttackFilterModel>();
            towerModel.GetAttackModels()[1].weapons[0].projectile.filters = towerModel.GetAttackModels()[1].weapons[0].projectile.filters.RemoveItemOfType<FilterModel, FilterFrozenBloonsModel>();
            towerModel.GetAttackModels()[1].weapons[0].projectile.GetBehavior<ProjectileFilterModel>().filters = towerModel.GetAttackModels()[1].weapons[0].projectile.GetBehavior<ProjectileFilterModel>().filters.RemoveItemOfType<FilterModel, FilterFrozenBloonsModel>();

            var abilityProjectile = towerModel.GetAttackModels()[1].weapons[0].projectile.Duplicate();
            abilityProjectile.AddBehavior(towerModel.GetAttackModels()[0].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>());
            abilityProjectile.id = "SnowstormProjectile";
            abilityProjectile.pierce = 9999999f;
            abilityProjectile.radius = 9999999f;
            abilityProjectile.filters = model050.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.filters;
            abilityProjectile.GetBehavior<ProjectileFilterModel>().filters = abilityProjectile.filters;
            abilityProjectile.GetBehavior<FreezeModel>().Lifespan = 12f;
            abilityProjectile.GetBehavior<FreezeModel>().mutationId = "AbsoluteZero:Regular:Freeze";
            abilityProjectile.AddBehavior(towerModel.GetAttackModels()[0].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModifierForTagModel>());
            abilityProjectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<EmitOnDestroyModel>().projectile.RemoveBehavior<CreateProjectileOnContactModel>();

            towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile = abilityProjectile;
            towerModel.GetAbility().Cooldown = 13f;

            towerModel.GetAttackModels()[1].weapons[0].projectile.RemoveBehavior<AddBehaviorToBloonModel>();
        }

        public class Wind : ModDisplay
        {
            public override string BaseDisplay => "12e86af1959e20a46a959673bbf077e6";

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                node.PrintInfo();
                var renderers = node.GetComponentsInChildren<SpriteRenderer>();
                foreach (var renderer in renderers)
                {
                    renderer.transform.localScale *= 3f;
                }
            }
        }
    }
}