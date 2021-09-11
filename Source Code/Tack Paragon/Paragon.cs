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
using Assets.Scripts.Unity;
using Assets.Scripts.Utils;
using BTD_Mod_Helper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;

namespace Tack_Paragon
{
    class Paragon
    {
        public static TowerModel towerModel;
        public static UpgradeModel upgradeModel;

        public static string baseTower = Main.baseTower;

        static Paragon()
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
                icon: new SpriteReference(guid: "625ae9a145fb447399e7a6b37235f75d"),
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
            towerModel.range = 40f;

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

            towerModel.targetTypes = model500.targetTypes;
        }

        static void AddCommonBehaviors()
        {
            var model500 = Game.instance.model.GetTowerFromId($"{baseTower}-500").Duplicate();

            towerModel.mods = towerModel.mods.AddTo(model500.mods[5]);
            towerModel.mods = towerModel.mods.AddTo(model500.mods[6]);
            towerModel.mods = towerModel.mods.AddTo(model500.mods[7]);

            var attackBehaviors = new Il2CppReferenceArray<Model>(0);
            attackBehaviors = attackBehaviors.AddTo(model500.GetAttackModel().GetBehavior<AttackFilterModel>());
            attackBehaviors = attackBehaviors.AddTo(model500.GetAttackModel().GetBehavior<TargetCloseModel>());

            var commonWeapon = new WeaponModel(
                name: $"{baseTower}_Common_WeaponModel",
                animation: 1,
                ejectX: 0,
                ejectY: 0,
                ejectZ: 0f,
                animationOffset: 0f,
                fireWithoutTarget: false,
                fireBetweenRounds: false,
                behaviors: null,
                useAttackPosition: false,
                startInCooldown: false,
                customStartCooldown: 0f,
                animateOnMainAttack: false
            );

            towerModel.AddBehavior(new AttackModel(
                name: $"{baseTower}_AttackModel",
                weapons: new Il2CppReferenceArray<WeaponModel>(new WeaponModel[] { commonWeapon }),
                range: 40f,
                behaviors: attackBehaviors,
                targetProvider: null,
                offsetX: 0f,
                offsetY: 0f,
                offsetZ: 0f,
                attackThroughWalls: false,
                fireWithoutTarget: false,
                framesBeforeRetarget: 0,
                addsToSharedGrid: true,
                sharedGridRange: 0f
            ));
        }

        static void CombineUniqueBehaviors()
        {
            var model500 = Game.instance.model.GetTowerFromId($"{baseTower}-500").Duplicate();
            var model050 = Game.instance.model.GetTowerFromId($"{baseTower}-050").Duplicate();
            var model005 = Game.instance.model.GetTowerFromId($"{baseTower}-005").Duplicate();

            towerModel.GetWeapon().Rate = model005.GetWeapon().Rate;
            towerModel.GetWeapon().emission = model005.GetWeapon().emission;
            towerModel.GetWeapon().projectile = model005.GetWeapon().projectile;

            towerModel.AddBehavior(model050.GetAbility());
        }

        static void CustomizeTower()
        {
            var model500 = Game.instance.model.GetTowerFromId($"{baseTower}-500").Duplicate();
            var model050 = Game.instance.model.GetTowerFromId($"{baseTower}-050").Duplicate();
            var model005 = Game.instance.model.GetTowerFromId($"{baseTower}-005").Duplicate();

            var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();

            towerModel.display = model005.display;
            towerModel.GetBehavior<DisplayModel>().display = model005.display;

            towerModel.AddBehavior(boomerangParagon.GetBehavior<ParagonTowerModel>());
            towerModel.GetBehavior<ParagonTowerModel>().displayDegreePaths.ForEach(path => path.assetPath = model005.display);
            towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());

            towerModel.GetWeapon().emission.Cast<ArcEmissionModel>().count = 32;
            towerModel.GetWeapon().emission.Cast<ArcEmissionModel>().sliceSize = 22.5f;

            towerModel.GetWeapon().projectile.display = "c184360c85b9d70499bb2fff7c77ecb2";
            towerModel.GetWeapon().projectile.scale = 2f;
            towerModel.GetWeapon().projectile.pierce = 60f;
            towerModel.GetWeapon().projectile.GetDamageModel().immuneBloonProperties = 0;
            towerModel.GetWeapon().projectile.GetBehavior<DisplayModel>().display = "c184360c85b9d70499bb2fff7c77ecb2"; // TackShooter-300 projectile
            towerModel.GetWeapon().projectile.GetBehavior<DisplayModel>().scale = 3f; 
            towerModel.GetWeapon().projectile.GetBehavior<TravelStraitModel>().Lifespan = 0.14f;
            towerModel.GetWeapon().projectile.GetBehavior<TravelStraitModel>().Speed = 300f;
            towerModel.GetWeapon().projectile.GetBehavior<ProjectileFilterModel>().filters[0].Cast<FilterInvisibleModel>().isActive = false;

            var wallOfFire = Game.instance.model.GetTowerFromId("WizardMonkey-020").GetAttackModels()[2].weapons[0].projectile.Duplicate();
            wallOfFire.scale = 0.1f;
            wallOfFire.filters[0].Cast<FilterInvisibleModel>().isActive = false;
            wallOfFire.GetBehavior<ProjectileFilterModel>().filters[0].Cast<FilterInvisibleModel>().isActive = false;
            wallOfFire.GetBehavior<DisplayModel>().scale = 0.1f;
            wallOfFire.GetBehavior<AgeModel>().lifespan = 0.1f;
            wallOfFire.RemoveBehavior<CreateEffectOnExhaustedModel>();

            var createProjectileModel = new CreateProjectileOnIntervalModel(
                name: "CreateWOFModel",
                projectile: wallOfFire,
                emission: new SingleEmissionModel(name: "Emission", null),
                intervalFrames: 3,
                onlyIfHasTarget: false,
                range: 60,
                targetType: ""
            );
            towerModel.GetWeapon().projectile.AddBehavior(createProjectileModel);

            var meteor = model500.GetAttackModels()[1].weapons[0].projectile.Duplicate();
            meteor.display = "83e667b55bc3de24d9a9e5b0256438a7"; // Lord Phoenix Meteor Display
            meteor.GetBehavior<DisplayModel>().display = meteor.display;
            meteor.pierce = 50;
            meteor.filters = meteor.filters.RemoveItemOfType<FilterModel, FilterAllExceptTargetModel>();
            meteor.GetBehavior<ProjectileFilterModel>().filters = meteor.GetBehavior<ProjectileFilterModel>().filters.RemoveItemOfType<FilterModel, FilterAllExceptTargetModel>();
            meteor.RemoveBehavior<TrackTargetModel>();
            meteor.RemoveBehavior<CreateEffectOnExhaustFractionModel>();
            towerModel.GetAbility().GetBehavior<ActivateAttackModel>().Lifespan = 5f;
            towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile = meteor;
            towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].Rate = 0.18f;
            towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].GetBehavior<SpinModel>().rotationPerSecond = 480;
        }
    }
}
