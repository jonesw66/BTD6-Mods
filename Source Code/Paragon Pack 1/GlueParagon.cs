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
using BTD_Mod_Helper.Extensions;
using Assets.Scripts.Models.Bloons.Behaviors;
using Assets.Scripts.Models.Bloons;

namespace Paragon_Pack_1
{
    static class GlueParagon
    {
        public static TowerModel towerModel;
        public static UpgradeModel upgradeModel;

        public static string baseTower = "GlueGunner";

        static GlueParagon()
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
                cost: 800000,
                xpCost: 0,
                icon: new SpriteReference(guid: "745c6562c66a5764dad63eb97ef95c50"), // Super Glue
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
            towerModel.range = 80f;

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
            towerModel.mods = towerModel.mods.AddTo(model500.mods[5]);
            towerModel.mods = towerModel.mods.AddTo(model500.mods[6]);

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

            var attackBehaviors = new Il2CppReferenceArray<Model>(0);
            attackBehaviors = attackBehaviors.AddTo(model500.GetAttackModel().GetBehavior<RotateToTargetModel>());
            attackBehaviors = attackBehaviors.AddTo(model500.GetAttackModel().GetBehavior<TargetFirstModel>());
            attackBehaviors = attackBehaviors.AddTo(model500.GetAttackModel().GetBehavior<TargetLastModel>());
            attackBehaviors = attackBehaviors.AddTo(model500.GetAttackModel().GetBehavior<TargetCloseModel>());
            attackBehaviors = attackBehaviors.AddTo(model500.GetAttackModel().GetBehavior<TargetStrongModel>());

            var commonWeapon = new WeaponModel(
                name: $"{baseTower}_Common_WeaponModel",
                animation: 1,
                animationOffset: 0.1f,
                fireWithoutTarget: false,
                fireBetweenRounds: false,
                behaviors: new Il2CppReferenceArray<WeaponBehaviorModel>(0),
                useAttackPosition: false,
                startInCooldown: false,
                customStartCooldown: 0f,
                animateOnMainAttack: false
            );

            towerModel.AddBehavior(new AttackModel(
                name: $"{baseTower}_AttackModel",
                weapons: new Il2CppReferenceArray<WeaponModel>(new WeaponModel[] { commonWeapon }),
                range: 80f,
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

            towerModel.mods = towerModel.mods.AddTo(model500.mods[0]);
            towerModel.mods = towerModel.mods.AddTo(model050.mods[0]);
            towerModel.mods = towerModel.mods.AddTo(model005.mods[0]);

            towerModel.AddBehavior(model050.GetBehavior<AbilityModel>());

            var attackFilterModel = model500.GetAttackModel().GetBehavior<AttackFilterModel>();
            attackFilterModel.filters[1].Cast<FilterGlueLevelModel>().glueLevel = 100;
            towerModel.GetAttackModel().AddBehavior(attackFilterModel);

            towerModel.GetWeapon().Rate = model005.GetWeapon().Rate;
            towerModel.GetWeapon().emission = model500.GetWeapon().emission;
            towerModel.GetWeapon().ejectX = model500.GetWeapon().ejectX;
            towerModel.GetWeapon().ejectY = model500.GetWeapon().ejectY;
            towerModel.GetWeapon().ejectZ = model500.GetWeapon().ejectZ;

            var pFilters = model500.GetWeapon().projectile.filters;
            pFilters[1].Cast<FilterGlueLevelModel>().glueLevel = 100;

            var pBehaviors = new Il2CppReferenceArray<Model>(0);
            pBehaviors = pBehaviors.AddTo(model500.GetWeapon().projectile.GetBehavior<TravelStraitModel>());
            
            var slowModel = model500.GetWeapon().projectile.GetBehavior<SlowModel>();
            slowModel.mutationId = "ParagonGlue";
            slowModel.glueLevel = 100;
            slowModel.lifespan = 24f;
            slowModel.Multiplier = 0.25f;
            pBehaviors = pBehaviors.AddTo(slowModel);

            var solverBehaviorModel = model500.GetWeapon().projectile.GetBehavior<AddBehaviorToBloonModel>();
            solverBehaviorModel.mutationId = "ParagonSolver";
            solverBehaviorModel.glueLevel = 100;
            pBehaviors = pBehaviors.AddTo(solverBehaviorModel);

            pBehaviors = pBehaviors.AddTo(model500.GetWeapon().projectile.GetBehavior<DamageModifierForTagModel>());
            pBehaviors = pBehaviors.AddTo(model005.GetWeapon().projectile.GetBehaviors<SlowModifierForTagModel>()[0]);
            pBehaviors = pBehaviors.AddTo(model005.GetWeapon().projectile.GetBehaviors<SlowModifierForTagModel>()[1]);

            var superGlueMoabsModel = model005.GetWeapon().projectile.GetBehaviors<SlowForBloonModel>()[0];
            superGlueMoabsModel.mutationId = "ParagonSuper";
            superGlueMoabsModel.glueLevel = 100;
            pBehaviors = pBehaviors.AddTo(superGlueMoabsModel);
            var superGlueMoabDdtModel = model005.GetWeapon().projectile.GetBehaviors<SlowForBloonModel>()[1];
            superGlueMoabDdtModel.mutationId = "ParagonSuper";
            superGlueMoabDdtModel.glueLevel = 100;
            pBehaviors = pBehaviors.AddTo(superGlueMoabDdtModel);
            var superGlueBfbModel = model005.GetWeapon().projectile.GetBehaviors<SlowForBloonModel>()[2];
            superGlueBfbModel.mutationId = "ParagonSuper";
            superGlueBfbModel.glueLevel = 100;
            pBehaviors = pBehaviors.AddTo(superGlueBfbModel);
            var superGlueZomgModel = model005.GetWeapon().projectile.GetBehaviors<SlowForBloonModel>()[3];
            superGlueZomgModel.mutationId = "ParagonSuper";
            superGlueZomgModel.glueLevel = 100;
            pBehaviors = pBehaviors.AddTo(superGlueZomgModel);

            var removeMutatorModel = model500.GetWeapon().projectile.GetBehavior<RemoveMutatorsFromBloonModel>();
            removeMutatorModel.mutatorIds = "SuperGlue,LiquifierDot,DissolverDot,CorrosiveDot,SolverDot";
            var mutatorIdList = new Il2CppStringArray(5);
            for (int i = 0; i < 4; i++) { mutatorIdList[i] = removeMutatorModel.mutatorIdList[i]; }
            mutatorIdList[4] = "SolverDot";
            removeMutatorModel.mutatorIdList = mutatorIdList;
            pBehaviors = pBehaviors.AddTo(removeMutatorModel);

            var filtersModel = model500.GetWeapon().projectile.GetBehavior<ProjectileFilterModel>();
            filtersModel.filters[1].Cast<FilterGlueLevelModel>().glueLevel = 100;
            pBehaviors = pBehaviors.AddTo(filtersModel);

            pBehaviors = pBehaviors.AddTo(model500.GetWeapon().projectile.GetBehavior<DisplayModel>());
            pBehaviors = pBehaviors.AddTo(model005.GetWeapon().projectile.GetBehaviors<AddBehaviorToBloonModel>()[2]);

            towerModel.GetWeapon().projectile = new ProjectileModel(
                display: "97f2427a81f436547b0a59f37fb689da",
                id: "Projectile",
                radius: 5f,
                vsBlockerRadius: 0f,
                pierce: 6f,
                maxPierce: 0f,
                behaviors: pBehaviors,
                filters: pFilters,
                ignoreBlockers: false,
                usePointCollisionWithBloons: false,
                canCollisionBeBlockedByMapLos: false,
                scale: 1f,
                collisionPasses: model500.GetWeapon().projectile.collisionPasses,
                dontUseCollisionChecker: false,
                checkCollisionFrames: 0,
                ignoreNonTargetable: false,
                ignorePierceExhaustion: false,
                saveId: null
            );
        }

        static void CustomizeTower()
        {
            var model050 = Game.instance.model.GetTowerFromId($"{baseTower}-050").Duplicate();

            var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();

            towerModel.display = model050.display;
            towerModel.GetBehavior<DisplayModel>().display = model050.display;

            towerModel.AddBehavior(boomerangParagon.GetBehavior<ParagonTowerModel>());
            towerModel.GetBehavior<ParagonTowerModel>().displayDegreePaths.ForEach(path => path.assetPath = model050.display);
            towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());

            towerModel.GetWeapon().emission.Cast<ArcEmissionModel>().count = 6;
            towerModel.GetWeapon().emission.Cast<ArcEmissionModel>().angle = 60;
            towerModel.GetWeapon().emission.Cast<ArcEmissionModel>().sliceSize = 10;

            towerModel.GetWeapon().projectile.GetBehavior<SlowModel>().Multiplier = 0.1f;
            towerModel.GetWeapon().projectile.GetBehaviors<SlowModifierForTagModel>()[0].slowMultiplier = 2f;
            towerModel.GetWeapon().projectile.GetBehaviors<SlowModifierForTagModel>()[1].slowMultiplier = 3.2f;
            towerModel.GetWeapon().projectile.GetBehaviors<SlowModifierForTagModel>()[1].resetToUnmodified = false;

            towerModel.GetWeapon().projectile.GetBehaviors<SlowForBloonModel>()[0].layers = 999;
            towerModel.GetWeapon().projectile.GetBehaviors<SlowForBloonModel>()[1].layers = 999;
            towerModel.GetWeapon().projectile.GetBehaviors<SlowForBloonModel>()[2].Multiplier = 0f;
            towerModel.GetWeapon().projectile.GetBehaviors<SlowForBloonModel>()[2].Lifespan = 4f;
            towerModel.GetWeapon().projectile.GetBehaviors<SlowForBloonModel>()[2].layers = 999;
            towerModel.GetWeapon().projectile.GetBehaviors<SlowForBloonModel>()[3].Multiplier = 0.02f;
            towerModel.GetWeapon().projectile.GetBehaviors<SlowForBloonModel>()[3].Lifespan = 2.5f;
            towerModel.GetWeapon().projectile.GetBehaviors<SlowForBloonModel>()[3].layers = 999;

            towerModel.GetWeapon().projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeCustomModel>().damage = 4;
            towerModel.GetWeapon().projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeCustomModel>().Interval = 0.4f;
            towerModel.GetWeapon().projectile.GetBehavior<DamageModifierForTagModel>().damageAddative = 2f;
            towerModel.GetWeapon().projectile.GetBehaviors<AddBehaviorToBloonModel>()[1].GetBehavior<DamageOverTimeModel>().damage = 100;
            towerModel.GetWeapon().projectile.GetBehaviors<AddBehaviorToBloonModel>()[1].lifespan = 3.5f;

            var baseProjectile = towerModel.GetWeapon().projectile.Duplicate();

            towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].Rate = 1f;
            towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile = baseProjectile;
            towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.display = null;
            towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.pierce = 9999999f;
            towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.radius = 999f;
            towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.filters[0].Cast<FilterInvisibleModel>().isActive = false;
            towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.AddBehavior(model050.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<AgeModel>());
            towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.AddBehavior(model050.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>());

            towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().perHitDamageAddition = 2;
            towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.RemoveBehavior<ProjectileFilterModel>();
        }
    }
}
