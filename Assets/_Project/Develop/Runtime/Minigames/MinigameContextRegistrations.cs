using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.StageFeature;
using Assets._Project.Develop.Runtime.Utilites.AssetsManagment;
using Assets._Project.Develop.Runtime.Utilites.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class MinigameContextRegistrations
    {
        private static MinigameInputArgs _inputArgs;

        public static void Process(DIContainer container, MinigameInputArgs inputArgs)
        {
            Debug.Log("Process registrations on minigame scene");

            _inputArgs = inputArgs;

            container.RegisterAsSingle(CreateEntitiesFactory);
            container.RegisterAsSingle(CreateEntitiesLifeContext);
            container.RegisterAsSingle(CreateMonoEntitiesFactory).NonLazy();

            container.RegisterAsSingle(CreateCollidersRegistryService);
            container.RegisterAsSingle(CreateBrainsFactory);
            container.RegisterAsSingle(CreateAIBrainContext);

            container.RegisterAsSingle<IInputService>(CreateDesktopInput);

            container.RegisterAsSingle(CreateMinigamesFactory);
            container.RegisterAsSingle(CreateMinigameStatesFactory);
            container.RegisterAsSingle(CreateGameplayStatesContext);

            // ui
        }

        private static MinigamesFactory CreateMinigamesFactory(DIContainer container)
        {
            return new MinigamesFactory(container);
        }

        private static MinigameStatesFactory CreateMinigameStatesFactory(DIContainer container)
        {
            return new MinigameStatesFactory(container);
        }

        private static MinigameStatesContext CreateGameplayStatesContext(DIContainer container)
        {
            return new MinigameStatesContext(
                container.Resolve<MinigameStatesFactory>()
                .CreateMinigameStateMachine(_inputArgs));
        }

        private static DesktopInput CreateDesktopInput(DIContainer container)
        {
            return new DesktopInput();
        }

        private static AIBrainsContext CreateAIBrainContext(DIContainer container)
        {
            return new AIBrainsContext();
        }

        private static BrainsFactory CreateBrainsFactory(DIContainer container)
        {
            return new BrainsFactory(container);
        }

        private static CollidersRegistryService CreateCollidersRegistryService(DIContainer container)
        {
            return new CollidersRegistryService();
        }

        private static MonoEntitiesFactory CreateMonoEntitiesFactory(DIContainer container)
        {
            return new MonoEntitiesFactory(
                container.Resolve<ResourcesAssetsLoader>(),
                container.Resolve<EntitiesLifeContext>(),
                container.Resolve<CollidersRegistryService>());
        }

        private static EntitiesLifeContext CreateEntitiesLifeContext(DIContainer container)
        {
            return new EntitiesLifeContext();
        }

        private static EntitiesFactory CreateEntitiesFactory(DIContainer container)
        {
            return new EntitiesFactory(container);
        }
    }
}
