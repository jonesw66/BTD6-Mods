﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MelonLoader;
using HarmonyLib;
using UnhollowerBaseLib;

using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Mods;
using Assets.Scripts.Models.Towers.Upgrades;
using Assets.Scripts.Unity;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Weapons;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Utils;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;

namespace Bomb_Paragon
{
    class BombParagon
    {
        public static TowerModel towerModel;
        public static UpgradeModel upgradeModel;

        static BombParagon()
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
                name: "BombShooter Paragon",
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
            var bomb = Game.instance.model.GetTower("BombShooter").Duplicate();

            towerModel = new TowerModel();

            towerModel.name = "BombShooter-Paragon";
            towerModel.display = "22ed7b557d493164495deaa7273d6651";
            towerModel.baseId = "BombShooter";

            towerModel.cost = 500000f;
            towerModel.towerSet = "Primary";
            towerModel.radius = 6f;
            towerModel.radiusSquared = 36f;
            towerModel.range = 80f;

            towerModel.ignoreBlockers = false;
            towerModel.isGlobalRange = false;
            towerModel.areaTypes = bomb.areaTypes;

            towerModel.tier = 6;
            towerModel.tiers = Game.instance.model.GetTowerFromId("DartMonkey-Paragon").tiers;

            towerModel.icon = bomb.icon;
            towerModel.portrait = bomb.portrait;
            towerModel.instaIcon = bomb.instaIcon;

            towerModel.ignoreTowerForSelection = false;
            towerModel.footprint = bomb.footprint.Duplicate();
            towerModel.dontDisplayUpgrades = false;
            towerModel.emoteSpriteSmall = null;
            towerModel.emoteSpriteLarge = null;
            towerModel.doesntRotate = false;

            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(0);
            var appliedUpgrades = new Il2CppStringArray(6);
            for (int upgrade = 0; upgrade < 5; upgrade++)
            {
                appliedUpgrades[upgrade] = Game.instance.model.GetTowerFromId("BombShooter-500").appliedUpgrades[upgrade];
            }
            appliedUpgrades[5] = "BombShooter Paragon";
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
            var bomb500 = Game.instance.model.GetTowerFromId("BombShooter-500").Duplicate();

            towerModel.mods = new Il2CppReferenceArray<ApplyModModel>(0);

            towerModel.mods = towerModel.mods.AddTo(bomb500.mods[0]);
            towerModel.mods = towerModel.mods.AddTo(bomb500.mods[1]);
            towerModel.mods = towerModel.mods.AddTo(bomb500.mods[2]);
            towerModel.mods = towerModel.mods.AddTo(bomb500.mods[3]);

            towerModel.AddBehavior(bomb500.GetBehavior<CreateEffectOnPlaceModel>());
            towerModel.AddBehavior(bomb500.GetBehavior<CreateSoundOnTowerPlaceModel>());
            towerModel.AddBehavior(bomb500.GetBehavior<CreateSoundOnUpgradeModel>());
            towerModel.AddBehavior(bomb500.GetBehavior<CreateSoundOnSellModel>());
            towerModel.AddBehavior(bomb500.GetBehavior<CreateEffectOnSellModel>());
            towerModel.AddBehavior(bomb500.GetBehavior<CreateEffectOnUpgradeModel>());
            towerModel.AddBehavior(bomb500.GetBehavior<DisplayModel>());

            towerModel.targetTypes = bomb500.targetTypes;
        }

        static void AddCommonBehaviors()
        {
            var bomb500 = Game.instance.model.GetTowerFromId("BombShooter-500").Duplicate();

            towerModel.mods = towerModel.mods.AddTo(bomb500.mods[4]);
            towerModel.mods = towerModel.mods.AddTo(bomb500.mods[5]);

            var attackBehaviors = new Il2CppReferenceArray<Model>(0);
            attackBehaviors = attackBehaviors.AddTo(bomb500.GetAttackModel().GetBehavior<RotateToTargetModel>());
            attackBehaviors = attackBehaviors.AddTo(bomb500.GetAttackModel().GetBehavior<AttackFilterModel>());
            attackBehaviors = attackBehaviors.AddTo(bomb500.GetAttackModel().GetBehavior<TargetFirstModel>());
            attackBehaviors = attackBehaviors.AddTo(bomb500.GetAttackModel().GetBehavior<TargetLastModel>());
            attackBehaviors = attackBehaviors.AddTo(bomb500.GetAttackModel().GetBehavior<TargetCloseModel>());
            attackBehaviors = attackBehaviors.AddTo(bomb500.GetAttackModel().GetBehavior<TargetStrongModel>());

            var commonWeapon = new WeaponModel(
                name: "BombParagon_Common_WeaponModel",
                animation: 1,
                animationOffset: 0,
                emission: bomb500.GetWeapon().emission,
                behaviors: new Il2CppReferenceArray<WeaponBehaviorModel>(0),
                customStartCooldown: 0
            );

            towerModel.AddBehavior(new AttackModel(
                name: "BombParagon_AttackModel",
                weapons: new Il2CppReferenceArray<WeaponModel>(new WeaponModel[] { commonWeapon }),
                range: 80f,
                behaviors: attackBehaviors,
                targetProvider: null,
                offsetX: 0,
                offsetY: 0,
                offsetZ: 0,
                attackThroughWalls: false,
                fireWithoutTarget: false,
                framesBeforeRetarget: 0,
                addsToSharedGrid: true,
                sharedGridRange: 0
            ));
        }

        static void CombineUniqueBehaviors()
        {
            var bomb500 = Game.instance.model.GetTowerFromId("BombShooter-500").Duplicate();
            var bomb050 = Game.instance.model.GetTowerFromId("BombShooter-050").Duplicate();
            var bomb005 = Game.instance.model.GetTowerFromId("BombShooter-005").Duplicate();

            towerModel.mods = towerModel.mods.AddTo(bomb050.mods[0]);
            towerModel.mods = towerModel.mods.AddTo(bomb005.mods[0]);

            towerModel.AddBehavior(bomb050.GetBehavior<AbilityModel>());
            towerModel.AddBehavior(bomb005.GetBehavior<AbilityModel>());

            towerModel.GetWeapon().Rate = bomb050.GetWeapon().Rate;
            towerModel.GetWeapon().ejectX = bomb005.GetWeapon().ejectX;
            towerModel.GetWeapon().ejectY = bomb005.GetWeapon().ejectY;
            towerModel.GetWeapon().ejectZ = bomb005.GetWeapon().ejectZ;

            var pBehaviors = new Il2CppReferenceArray<Model>(0);
            pBehaviors = pBehaviors.AddTo(bomb050.GetWeapon().projectile.GetBehavior<TravelStraitModel>());
            pBehaviors = pBehaviors.AddTo(bomb500.GetWeapon().projectile.GetBehavior<ProjectileFilterModel>());
            pBehaviors = pBehaviors.AddTo(bomb500.GetWeapon().projectile.GetBehavior<CreateEffectOnContactModel>());
            pBehaviors = pBehaviors.AddTo(bomb005.GetWeapon().projectile.GetBehavior<CreateEffectOnExhaustFractionModel>());
            pBehaviors = pBehaviors.AddTo(bomb500.GetWeapon().projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>());
            pBehaviors = pBehaviors.AddTo(bomb050.GetWeapon().projectile.GetBehavior<DisplayModel>());

            var firstBombCreateModel = bomb005.GetWeapon().projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>()[0];
            var firstBombProjectile = firstBombCreateModel.projectile;
            var firstClusterCreateModel = bomb005.GetWeapon().projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>()[1];
            var firstClusterProjectile = firstClusterCreateModel.projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>()[0].projectile;
            var secondClusterCreateModel = firstClusterCreateModel.projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>()[1];
            var secondClusterProjectile = secondClusterCreateModel.projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile;

            AddCrosspathBehaviorsToProjectile(ref firstBombProjectile);
            AddCrosspathBehaviorsToProjectile(ref firstClusterProjectile);
            AddCrosspathBehaviorsToProjectile(ref secondClusterProjectile);

            pBehaviors = pBehaviors.AddTo(firstBombCreateModel);
            pBehaviors = pBehaviors.AddTo(firstClusterCreateModel);

            towerModel.GetWeapon().projectile = new ProjectileModel(
                display: "e5edd901992846e409326a506d272633",
                id: "Recursive",
                radius: 5f,
                vsBlockerRadius: 0f,
                pierce: 1f,
                maxPierce: 1f,
                behaviors: pBehaviors,
                filters: bomb500.GetWeapon().projectile.filters,
                ignoreBlockers: false,
                usePointCollisionWithBloons: false,
                canCollisionBeBlockedByMapLos: false,
                scale: 1f,
                collisionPasses: bomb500.GetWeapon().projectile.collisionPasses,
                dontUseCollisionChecker: false,
                checkCollisionFrames: 0,
                ignoreNonTargetable: false,
                ignorePierceExhaustion: false,
                saveId: null
            );
        }

        static void CustomizeTower()
        {
            var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon");

            towerModel.display = "4a4a8d531e486ba43940bb673d1994ad";
            towerModel.GetBehavior<DisplayModel>().display = "4a4a8d531e486ba43940bb673d1994ad";

            towerModel.AddBehavior(boomerangParagon.GetBehavior<ParagonTowerModel>());
            towerModel.AddBehavior(boomerangParagon.GetBehaviors<ParagonAssetSwapModel>()[0]);
            towerModel.AddBehavior(boomerangParagon.GetBehaviors<ParagonAssetSwapModel>()[1]);
            towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());

            towerModel.GetAttackModel().GetBehavior<AttackFilterModel>().filters[0].Cast<FilterInvisibleModel>().isActive = false;
            towerModel.GetWeapon().projectile.filters[0].Cast<FilterInvisibleModel>().isActive = false;
            towerModel.GetWeapon().projectile.GetBehavior<ProjectileFilterModel>().filters[0].Cast<FilterInvisibleModel>().isActive = false;
            //towerModel.GetWeapon().projectile.GetBehavior<TravelStraitModel>().Speed = 320f;

            var mainProjectile = towerModel.GetWeapon().projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>()[0].projectile;
            //mainProjectile.pierce = 80f;
            //mainProjectile.radius = 40f;
            //mainProjectile.GetBehavior<DamageModel>().damage = 20f;
            //mainProjectile.GetBehavior<AgeModel>().lifespan = 0.15f;
            //mainProjectile.GetBehaviors<DamageModifierForTagModel>()[0].damageMultiplier = 1.5f;
            //mainProjectile.GetBehaviors<DamageModifierForTagModel>()[1].damageAddative = 10f;
            //mainProjectile.GetBehavior<SlowModel>().Lifespan = 3f;

            towerModel.GetWeapon().projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>()[1].projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>()[0].projectile = mainProjectile.Duplicate();
            towerModel.GetWeapon().projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>()[1].projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>()[1].emission = towerModel.GetWeapon().projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>()[1].emission.Duplicate();
            towerModel.GetWeapon().projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>()[1].projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>()[1].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile = mainProjectile.Duplicate();

            //towerModel.GetAbilites()[0].GetBehavior<ActivateAttackModel>().isOneShot = false;
            //towerModel.GetAbilites()[0].GetBehavior<ActivateAttackModel>().Lifespan = 1f;
            //towerModel.GetAbilites()[0].GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].Rate = 0.2f;
            //towerModel.GetAbilites()[0].GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].emission = Game.instance.model.GetTower("Adora").GetWeapon().emission;
            //towerModel.GetAbilites()[0].GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].emission.Cast<AdoraEmissionModel>().count = 5;
            //towerModel.GetAbilites()[0].GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].emission.Cast<AdoraEmissionModel>().angleBetween = 110f;
            //towerModel.GetAbilites()[0].GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.RemoveBehavior<TravelStraitModel>();
            //var adoraModel = new AdoraTrackTargetModel(
            //    name: "AbilityTrackModel",
            //    rotation: 15f,
            //    minimumSpeed: 300f,
            //    maximumSpeed: 500f,
            //    acceleration: 90f,
            //    lifespan: 1.5f,
            //    accelerateInAngle: 50f,
            //    startDeceleratingIfAngleGreaterThan: 70f
            //);
            //towerModel.GetAbilites()[0].GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.AddBehavior(adoraModel);

            //towerModel.GetAbilites()[1].activateOnPreLeak = true;
            //towerModel.GetAbilites()[1].activateOnLivesLost = false;
            //towerModel.GetAbilites()[1].cooldown = 1f;
            //towerModel.GetAbilites()[1].GetBehavior<ActivateAttackModel>().processOnActivate = true;
            //towerModel.GetAbilites()[1].GetBehavior<ActivateAttackModel>().attacks[0].weapons[1].projectile.GetBehavior<DamageModel>().damage = 3000f;
            //var immunityAbilityModel = Game.instance.model.GetTowerFromId("SuperMonkey-005").GetAbility().Duplicate();
            //immunityAbilityModel.GetBehavior<ImmunityModel>().lifespan = 0.2f;
            //immunityAbilityModel.GetBehavior<ImmunityModel>().effectModel = null;
            //immunityAbilityModel.RemoveBehavior<CreateEffectOnAbilityModel>();
            //towerModel.AddBehavior(immunityAbilityModel);
            //var turboAbility = Game.instance.model.GetTowerFromId("BoomerangMonkey-040").GetAbility().Duplicate();
            //turboAbility.GetBehavior<TurboModel>().projectileDisplay = null;
            //towerModel.AddBehavior(turboAbility);
        }

        static void AddCrosspathBehaviorsToProjectile(ref ProjectileModel projectile)
        {
            var bomb500 = Game.instance.model.GetTowerFromId("BombShooter-500").Duplicate();
            var bomb050 = Game.instance.model.GetTowerFromId("BombShooter-050").Duplicate();

            projectile.pierce = 60;
            projectile.filters[0].Cast<FilterInvisibleModel>().isActive = false;
            projectile.collisionPasses = bomb500.GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.collisionPasses;
            projectile.radius = 33.48f;
            projectile.hasDamageModifiers = true;
            projectile.GetBehavior<DamageModel>().damage = 12f;
            projectile.GetBehavior<AgeModel>().Lifespan = 0.1f;
            projectile.GetBehavior<ProjectileFilterModel>().filters[0].Cast<FilterInvisibleModel>().isActive = false;
            projectile.AddBehavior(bomb050.GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehaviors<DamageModifierForTagModel>()[0]);
            projectile.AddBehavior(bomb050.GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehaviors<DamageModifierForTagModel>()[1]);
            projectile.AddBehavior(bomb500.GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<SlowModel>());
        }
    }
}
