using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

namespace Sniper_Paragon
{
    public class SniperParagon
    {
        public static TowerModel towerModel;
        public static UpgradeModel upgradeModel;

        static SniperParagon()
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
                name: "SniperMonkey Paragon",
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
            var sniper = Game.instance.model.GetTower("SniperMonkey").Duplicate();

            towerModel = new TowerModel();

            towerModel.name = "SniperMonkey-Paragon";
            towerModel.display = "22ed7b557d493164495deaa7273d6651";
            towerModel.baseId = "SniperMonkey";

            towerModel.cost = 500000f;
            towerModel.towerSet = "Military";
            towerModel.radius = 6f;
            towerModel.radiusSquared = 36f;
            towerModel.range = 22f;

            towerModel.ignoreBlockers = true;
            towerModel.isGlobalRange = true;
            towerModel.areaTypes = sniper.areaTypes;

            towerModel.tier = 6;
            towerModel.tiers = Game.instance.model.GetTowerFromId("DartMonkey-Paragon").tiers;

            towerModel.icon = sniper.icon;
            towerModel.portrait = sniper.portrait;
            towerModel.instaIcon = sniper.instaIcon;

            towerModel.ignoreTowerForSelection = false;
            towerModel.footprint = sniper.footprint.Duplicate();
            towerModel.dontDisplayUpgrades = false;
            towerModel.emoteSpriteSmall = null;
            towerModel.emoteSpriteLarge = null;
            towerModel.doesntRotate = false;

            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(0);
            var appliedUpgrades = new Il2CppStringArray(6);
            for (int upgrade = 0; upgrade < 5; upgrade++)
            {
                appliedUpgrades[upgrade] = Game.instance.model.GetTowerFromId("SniperMonkey-500").appliedUpgrades[upgrade];
            }
            appliedUpgrades[5] = "SniperMonkey Paragon";
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
            var sniper500 = Game.instance.model.GetTowerFromId("SniperMonkey-500").Duplicate();

            towerModel.mods = new Il2CppReferenceArray<ApplyModModel>(0);
            towerModel.mods = towerModel.mods.AddTo(sniper500.mods[2]);
            towerModel.mods = towerModel.mods.AddTo(sniper500.mods[3]);
            towerModel.mods = towerModel.mods.AddTo(sniper500.mods[4]);
            towerModel.mods = towerModel.mods.AddTo(sniper500.mods[5]);

            towerModel.AddBehavior(sniper500.GetBehavior<CreateEffectOnPlaceModel>());
            towerModel.AddBehavior(sniper500.GetBehavior<CreateSoundOnTowerPlaceModel>());
            towerModel.AddBehavior(sniper500.GetBehavior<CreateSoundOnUpgradeModel>());
            towerModel.AddBehavior(sniper500.GetBehavior<CreateSoundOnSellModel>());
            towerModel.AddBehavior(sniper500.GetBehavior<CreateEffectOnSellModel>());
            towerModel.AddBehavior(sniper500.GetBehavior<CreateEffectOnUpgradeModel>());
            towerModel.AddBehavior(sniper500.GetBehavior<DisplayModel>());

            towerModel.targetTypes = new Il2CppReferenceArray<TargetType>(0);
            towerModel.targetTypes = towerModel.targetTypes.AddTo(sniper500.targetTypes[0]);
            towerModel.targetTypes = towerModel.targetTypes.AddTo(sniper500.targetTypes[1]);
            towerModel.targetTypes = towerModel.targetTypes.AddTo(sniper500.targetTypes[2]);
            towerModel.targetTypes = towerModel.targetTypes.AddTo(sniper500.targetTypes[3]);
        }

        static void AddCommonBehaviors()
        {
            var sniper500 = Game.instance.model.GetTowerFromId("SniperMonkey-500").Duplicate();

            var attackBehaviors = new Il2CppReferenceArray<Model>(0);
            attackBehaviors = attackBehaviors.AddTo(sniper500.GetAttackModel().GetBehavior<RotateToTargetModel>());
            attackBehaviors = attackBehaviors.AddTo(sniper500.GetAttackModel().GetBehavior<TargetFirstModel>());
            attackBehaviors = attackBehaviors.AddTo(sniper500.GetAttackModel().GetBehavior<TargetLastModel>());
            attackBehaviors = attackBehaviors.AddTo(sniper500.GetAttackModel().GetBehavior<TargetCloseModel>());
            attackBehaviors = attackBehaviors.AddTo(sniper500.GetAttackModel().GetBehavior<TargetStrongModel>());
            attackBehaviors = attackBehaviors.AddTo(sniper500.GetAttackModel().GetBehavior<CheckTargetsWithoutOffsetsModel>());

            var commonWeapon = new WeaponModel(
                name: "SniperParagon_Common_WeaponModel",
                animation: 1,
                animationOffset: 0f,
                fireWithoutTarget: false,
                fireBetweenRounds: false,
                emission: sniper500.GetWeapon().emission,
                behaviors: sniper500.GetWeapon().behaviors,
                useAttackPosition: false,
                startInCooldown: false,
                customStartCooldown: 0f,
                animateOnMainAttack: false
            );

            towerModel.AddBehavior(new AttackModel(
                name: "SniperParagon_AttackModel",
                weapons: new Il2CppReferenceArray<WeaponModel>(new WeaponModel[] { commonWeapon }),
                range: 9999999f,
                behaviors: attackBehaviors,
                targetProvider: null,
                offsetX: 0f,
                offsetY: 0f,
                offsetZ: 0f,
                attackThroughWalls: true,
                fireWithoutTarget: false,
                framesBeforeRetarget: 0,
                addsToSharedGrid: true,
                sharedGridRange: 20f
            ));
        }

        static void CombineUniqueBehaviors()
        {
            var sniper500 = Game.instance.model.GetTowerFromId("SniperMonkey-500").Duplicate();
            var sniper050 = Game.instance.model.GetTowerFromId("SniperMonkey-050").Duplicate();
            var sniper005 = Game.instance.model.GetTowerFromId("SniperMonkey-005").Duplicate();

            towerModel.mods = towerModel.mods.AddTo(sniper500.mods[0]);
            towerModel.mods = towerModel.mods.AddTo(sniper500.mods[1]);
            towerModel.mods = towerModel.mods.AddTo(sniper500.mods[6]);
            towerModel.mods = towerModel.mods.AddTo(sniper500.mods[7]);
            towerModel.mods = towerModel.mods.AddTo(sniper500.mods[8]);
            towerModel.mods = towerModel.mods.AddTo(sniper005.mods[0]);
            towerModel.mods = towerModel.mods.AddTo(sniper005.mods[1]);

            towerModel.targetTypes = towerModel.targetTypes.AddTo(sniper050.targetTypes[4]);

            towerModel.AddBehavior(sniper050.GetBehavior<SwitchTargetSupplierOnUpgradeModel>());
            towerModel.AddBehavior(sniper050.GetBehavior<AbilityModel>());
            towerModel.AddBehavior(sniper050.GetBehavior<TargetSupplierSupportModel>());
            towerModel.AddBehavior(sniper050.GetBehavior<RateSupportModel>());
            towerModel.AddBehavior(sniper005.GetBehavior<AbilityModel>());

            towerModel.GetAttackModel().AddBehavior(sniper050.GetAttackModel().GetBehavior<AttackFilterModel>());
            towerModel.GetAttackModel().AddBehavior(sniper050.GetAttackModel().GetBehavior<TargetEliteTargettingModel>());

            towerModel.GetWeapon().Rate = sniper005.GetWeapon().Rate;
            towerModel.GetWeapon().ejectX = sniper500.GetWeapon().ejectX;
            towerModel.GetWeapon().ejectY = sniper500.GetWeapon().ejectY;
            towerModel.GetWeapon().ejectZ = sniper500.GetWeapon().ejectZ;

            var pBehaviors = new Il2CppReferenceArray<Model>(0);
            pBehaviors = pBehaviors.AddTo(sniper050.GetWeapon().projectile.GetBehavior<ProjectileFilterModel>());
            pBehaviors = pBehaviors.AddTo(sniper500.GetWeapon().projectile.GetBehavior<InstantModel>());
            pBehaviors = pBehaviors.AddTo(sniper050.GetWeapon().projectile.GetBehavior<AgeModel>());
            pBehaviors = pBehaviors.AddTo(sniper500.GetWeapon().projectile.GetBehavior<SlowMaimMoabModel>());
            pBehaviors = pBehaviors.AddTo(sniper050.GetWeapon().projectile.GetBehavior<EmitOnDamageModel>());
            pBehaviors = pBehaviors.AddTo(sniper500.GetWeapon().projectile.GetBehavior<DamageModel>());
            pBehaviors = pBehaviors.AddTo(sniper500.GetWeapon().projectile.GetBehavior<DamageModifierForTagModel>());
            pBehaviors = pBehaviors.AddTo(sniper050.GetWeapon().projectile.GetBehavior<DamageModifierForTagModel>());
            pBehaviors = pBehaviors.AddTo(sniper050.GetWeapon().projectile.GetBehavior<RetargetOnContactModel>());
            pBehaviors = pBehaviors.AddTo(sniper050.GetWeapon().projectile.GetBehavior<CreateEffectFromCollisionToCollisionModel>());
            pBehaviors = pBehaviors.AddTo(sniper050.GetWeapon().projectile.GetBehavior<CreateEffectOnContactModel>());
            pBehaviors = pBehaviors.AddTo(sniper500.GetWeapon().projectile.GetBehavior<DisplayModel>());

            towerModel.GetWeapon().projectile = new ProjectileModel(
                display: null,
                id: "Projectile",
                radius: 0f,
                vsBlockerRadius: 0f,
                pierce: 5f,
                maxPierce: 0f,
                behaviors: pBehaviors,
                filters: sniper050.GetWeapon().projectile.filters,
                ignoreBlockers: true,
                usePointCollisionWithBloons: false,
                canCollisionBeBlockedByMapLos: false,
                scale: 1f,
                collisionPasses: sniper050.GetWeapon().projectile.collisionPasses,
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

            towerModel.GetBehaviors<AbilityModel>()[0].Cooldown = 25f;
            towerModel.GetBehaviors<AbilityModel>()[0].maxActivationsPerRound = 0;
            towerModel.GetBehaviors<AbilityModel>()[0].GetBehavior<ActivateAttackModel>().Lifespan = 2.0f;
            towerModel.GetBehaviors<AbilityModel>()[0].GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].Rate = 0.4f;
            towerModel.GetBehaviors<AbilityModel>()[0].GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].Rate = 0.4f;

            towerModel.GetWeapon().Rate = 0.05f;
            towerModel.GetWeapon().projectile.pierce = 10;
            towerModel.GetWeapon().projectile.CapPierce(0);

            var maimModel = towerModel.GetWeapon().projectile.GetBehavior<SlowMaimMoabModel>();
            maimModel.moabDuration *= 1.2f;
            maimModel.bfbDuration *= 1.2f;
            maimModel.zomgDuration *= 1.2f;
            maimModel.ddtDuration *= 1.2f;
            maimModel.badDuration *= 1.2f;
            maimModel.bloonPerHitDamageAddition = 7f;

            var emitModel = towerModel.GetWeapon().projectile.GetBehavior<EmitOnDamageModel>();
            emitModel.emission.Cast<ArcEmissionModel>().offsetStart = 11.25f;
            emitModel.emission.Cast<ArcEmissionModel>().count = 7;
            emitModel.projectile.pierce = 10f;
            emitModel.projectile.GetBehavior<DamageModel>().damage = 2f;
            emitModel.projectile.GetBehavior<DamageModel>().immuneBloonProperties = 0;
            emitModel.projectile.GetBehavior<TravelStraitModel>().Speed = 200f;

            towerModel.GetWeapon().projectile.GetBehavior<DamageModel>().damage = 100f;
            towerModel.GetWeapon().projectile.GetBehavior<RetargetOnContactModel>().distance = 999f;
        }
    }
}
