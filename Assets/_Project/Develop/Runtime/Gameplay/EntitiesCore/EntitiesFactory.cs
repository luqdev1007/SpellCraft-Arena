using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot;
using Assets._Project.Develop.Runtime.Gameplay.Features.ContactTakeDamage;
using Assets._Project.Develop.Runtime.Gameplay.Features.Energy;
using Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle;
using Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.Sensors;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeleportFeature;
using Assets._Project.Develop.Runtime.Utilites;
using Assets._Project.Develop.Runtime.Utilites.Conditions;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.EntitiesCore
{
    public class EntitiesFactory
    {
        private readonly DIContainer _container;
        private readonly EntitiesLifeContext _entitiesLifeContext;

        private readonly MonoEntitiesFactory _monoEntitiesFactory;

        private readonly CollidersRegistryService _collidersRegistryService;

        public EntitiesFactory(DIContainer container)
        {
            _container = container;
            _entitiesLifeContext = container.Resolve<EntitiesLifeContext>();
            _monoEntitiesFactory = container.Resolve<MonoEntitiesFactory>();
            _collidersRegistryService = container.Resolve<CollidersRegistryService>();
        }

        public Entity CreateMagicOrb(Vector3 position)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, "Entities/MagicOrb");

            entity
                .AddMaxHealth(new ReactiveVariable<float>(7))
                .AddCurrentHealth(new ReactiveVariable<float>(7))
                .AddIsDead()
                .AddInDeathProcess()
                .AddDeathProcessInitialTime(new ReactiveVariable<float>(3))
                .AddDeathProcessCurrentTime()
                .AddTakeDamageRequest()
                .AddTakeDamageEvent()
                .AddEnergyCurrentValue(new ReactiveVariable<float>(100))
                .AddEnergyMaxValue(new ReactiveVariable<float>(100))
                .AddTeleportInitialCooldown(new ReactiveVariable<float>(3))
                .AddTeleportCurrentCooldown(new ReactiveVariable<float>(3))
                .AddAmountOfEnergyForTeleport(new ReactiveVariable<float>(25))
                .AddIsTeleportCooldownReady(new ReactiveVariable<bool>(false))
                .AddTeleportRequest()
                .AddAmountOfRestoreEnergy(new ReactiveVariable<float>(entity.EnergyMaxValue.Value * 0.02f))
                .AddTimeToRestoreEnergy(new ReactiveVariable<float>(1))
                .AddAttackDamage(new ReactiveVariable<float>(1))
                .AddAttackRange(new ReactiveVariable<float>(5))
                .AddMaxTeleportRange(new ReactiveVariable<float>(4));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentHealth.Value <= 0));

            ICompositeCondition canApplyDamage = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canTeleport = new CompositeCondition()
                .Add(new FuncCondition(() => entity.EnergyCurrentValue.Value >= entity.AmountOfEnergyForTeleport.Value))
                .Add(new FuncCondition(() => entity.IsDead.Value == false))
                .Add(new FuncCondition(() => entity.InDeathProcess.Value == false))
                .Add(new FuncCondition(() => entity.IsTeleportCooldownReady.Value == true));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == true))
                .Add(new FuncCondition(() => entity.InDeathProcess.Value == false));

            ICompositeCondition canRestoreEnergy = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false))
                .Add(new FuncCondition(() => entity.InDeathProcess.Value == false));

            entity
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease)
                .AddCanTeleport(canTeleport)
                .AddCanRestoreEnergy(canRestoreEnergy)
                .AddCanApplyDamage(canApplyDamage);

            entity
                  .AddSystem(new DeathSystem())
                  .AddSystem(new DeathProcessTimerSystem())
                  .AddSystem(new SelfReleaseSystem(_entitiesLifeContext))
                  .AddSystem(new TeleportCooldownSystem())
                  .AddSystem(new RestoreEnergySystem())
                  .AddSystem(new DealDamageInRangeAfterTeleportSystem(_collidersRegistryService))
                  .AddSystem(new DisableCollidersOnDeathSystem())
                  .AddSystem(new ApplyDamageSystem());

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity CreateHero(Vector3 position, HeroConfig config)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, "Entities/Hero");

            entity
                .AddMoveDirection()
                .AddMoveSpeed(new ReactiveVariable<float>(config.MoveSpeed))
                .AddIsMoving()
                .AddRotationSpeed(new ReactiveVariable<float>(config.RotationSpeed))
                .AddRotationDirection()
                .AddMaxHealth(new ReactiveVariable<float>(config.MaxHealth))
                .AddCurrentHealth(new ReactiveVariable<float>(config.MaxHealth))
                .AddIsDead()
                .AddInDeathProcess()
                .AddDeathProcessInitialTime(new ReactiveVariable<float>(config.DeathProcessTime))
                .AddDeathProcessCurrentTime()
                .AddTakeDamageRequest()
                .AddTakeDamageEvent()
                .AddAttackProcessInitialTime(new ReactiveVariable<float>(config.AttackProcessTime))
                .AddAttackProcessCurrentTime()
                .AddInAttackProcess()
                .AddStartAttackRequest()
                .AddStartAttackEvent()
                .AddEndAttackEvent()
                .AddAttackDelayTime(new ReactiveVariable<float>(config.AttackDelayTime))
                .AddAttackDelayEndEvent()
                .AddInstantAttackDamage(new ReactiveVariable<float>(config.InstantAttackDamage))
                .AddAttackCanceledEvent()
                .AddAttackCooldownInitialTime(new ReactiveVariable<float>(config.AttackCooldown))
                .AddAttackCooldownCurrentTime()
                .AddInAttackCooldown();

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canRotate = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false))
                .Add(new FuncCondition(() => entity.InAttackProcess.Value == false));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentHealth.Value <= 0));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == true))
                .Add(new FuncCondition(() => entity.InDeathProcess.Value == false));

            ICompositeCondition canApplyDamage = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canStartAttack = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false))
                .Add(new FuncCondition(() => entity.InAttackProcess.Value == false))
                .Add(new FuncCondition(() => entity.IsMoving.Value == false))
                .Add(new FuncCondition(() => entity.InAttackCooldown.Value == false));

            ICompositeCondition mustCancelAttack = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => entity.IsDead.Value == true))
                .Add(new FuncCondition(() => entity.IsMoving.Value == true));

            entity
                .AddCanMove(canMove)
                .AddCanRotate(canRotate)
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease)
                .AddCanApplyDamage(canApplyDamage)
                .AddCanStartAttack(canStartAttack)
                .AddMustCancelAttack(mustCancelAttack);

            entity.AddSystem(new RigidbodyMovementSystem())
                  .AddSystem(new RigidbodyRotationSystem())
                  .AddSystem(new AttackCancelSystem())
                  .AddSystem(new StartAttackSystem())
                  .AddSystem(new AttackProcessTimerSystem())
                  .AddSystem(new AttackDelayEndTriggerSystem())
                  .AddSystem(new InstantShootSystem(this))
                  .AddSystem(new EndAttackSystem())
                  .AddSystem(new AttackCooldownTimerSystem())
                  .AddSystem(new ApplyDamageSystem())
                  .AddSystem(new DeathSystem())
                  .AddSystem(new DisableCollidersOnDeathSystem())
                  .AddSystem(new DeathProcessTimerSystem())
                  .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            return entity;
        }

        public Entity CreateGhost(Vector3 position, GhostConfig config)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, "Entities/Ghost");

            entity
                .AddMoveDirection()
                .AddMoveSpeed(new ReactiveVariable<float>(config.MoveSpeed))
                .AddIsMoving()
                .AddRotationSpeed(new ReactiveVariable<float>(config.RotationSpeed))
                .AddRotationDirection()
                .AddMaxHealth(new ReactiveVariable<float>(config.MaxHealth))
                .AddCurrentHealth(new ReactiveVariable<float>(config.MaxHealth))
                .AddIsDead()
                .AddInDeathProcess()
                .AddDeathProcessInitialTime(new ReactiveVariable<float>(config.DeathProcessTime))
                .AddDeathProcessCurrentTime()
                .AddTakeDamageRequest()
                .AddTakeDamageEvent()
                .AddContactsDetectingMask(LayersAPI.LayerMaskCharacters)
                .AddContactCollidersBuffer(new Buffer<Collider>(64))
                .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                .AddBodyContactDamage(new ReactiveVariable<float>(config.BodyContactDamage));

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canRotate = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentHealth.Value <= 0));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == true))
                .Add(new FuncCondition(() => entity.InDeathProcess.Value == false));

            ICompositeCondition canApplyDamage = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            entity
                .AddCanMove(canMove)
                .AddCanRotate(canRotate)
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease)
                .AddCanApplyDamage(canApplyDamage);

            entity.AddSystem(new RigidbodyMovementSystem())
                  .AddSystem(new RigidbodyRotationSystem())
                  .AddSystem(new ApplyDamageSystem())
                  .AddSystem(new DeathSystem())
                  .AddSystem(new DeathProcessTimerSystem())
                  .AddSystem(new SelfReleaseSystem(_entitiesLifeContext))
                  .AddSystem(new DisableCollidersOnDeathSystem())
                  .AddSystem(new BodyContactDetectingSystem())
                  .AddSystem(new BodyContactsEntitiesFilterSystem(_collidersRegistryService))
                  .AddSystem(new DealDamageOnContactSystem());

            return entity;
        }

        public Entity CreateFireballProjectile(Vector3 position, Vector3 direction, float damage, Entity owner)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, "Entities/FireballProjectile");

            entity
                .AddMoveDirection(new ReactiveVariable<Vector3>(direction))
                .AddMoveSpeed(new ReactiveVariable<float>(10))
                .AddIsMoving()
                .AddRotationSpeed(new ReactiveVariable<float>(9999))
                .AddRotationDirection(new ReactiveVariable<Vector3>(direction))
                .AddIsDead()
                .AddContactsDetectingMask(LayersAPI.LayerMaskCharacters | LayersAPI.LayerMaskEnviroment)
                .AddContactCollidersBuffer(new Buffer<Collider>(64))
                .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                .AddBodyContactDamage(new ReactiveVariable<float>(damage))
                .AddDeathMask(LayersAPI.LayerMaskEnviroment)
                .AddIsTouchDeathMask()
                .AddIsTouchAnotherTeam()
                .AddTeam(new ReactiveVariable<Teams>(owner.Team.Value));

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canRotate = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => entity.IsTouchDeathMask.Value == true))
                .Add(new FuncCondition(() => entity.IsTouchAnotherTeam.Value == true));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == true));

            entity
                .AddCanMove(canMove)
                .AddCanRotate(canRotate)
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease);

            entity.AddSystem(new RigidbodyMovementSystem())
                  .AddSystem(new RigidbodyRotationSystem())
                  .AddSystem(new DeathSystem())
                  .AddSystem(new DisableCollidersOnDeathSystem())
                  .AddSystem(new SelfReleaseSystem(_entitiesLifeContext))
                  .AddSystem(new BodyContactDetectingSystem())
                  .AddSystem(new BodyContactsEntitiesFilterSystem(_collidersRegistryService))
                  .AddSystem(new DealDamageOnContactSystem())
                  .AddSystem(new DeathMaskTouchDetectorSystem())
                  .AddSystem(new AnotherTeamTouchDetectorSystem());

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity CreateContactTrigger(Vector3 position)
        {
            Entity entity = CreateEmpty();   

            _monoEntitiesFactory.Create(entity, position, "Entities/ContactTrigger");

            entity
                .AddContactsDetectingMask(LayersAPI.LayerMaskCharacters)
                .AddContactCollidersBuffer(new Buffer<Collider>(64))
                .AddContactEntitiesBuffer(new Buffer<Entity>(64));

            entity
                  .AddSystem(new BodyContactDetectingSystem())
                  .AddSystem(new BodyContactsEntitiesFilterSystem(_collidersRegistryService));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        private Entity CreateEmpty() => new Entity();
    }
}
