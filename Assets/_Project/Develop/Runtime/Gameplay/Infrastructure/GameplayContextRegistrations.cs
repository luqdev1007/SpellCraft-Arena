using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Gameplay.TypeMode;
using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilites.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.DataProviders;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayContextRegistrations
    {
        private static GameplayInputArgs _inputArgs;

        public static void Process(DIContainer container, GameplayInputArgs inputArgs)
        {
            Debug.Log("Process registrations on gameplay scene");

            _inputArgs = inputArgs;

            container.RegisterAsSingle(CreateTypeModeGeneratorService);
            container.RegisterAsSingle(CreateTypeModeHandler);
            container.RegisterAsSingle(CreateTypeModeInputService);
            container.RegisterAsSingle(CreateTypeModeResultService);
        }

        private static TypeModeResultService CreateTypeModeResultService(DIContainer container)
        {
            return new TypeModeResultService(container.Resolve<SceneSwitcherService>(),
                container.Resolve<ICoroutinesPerformer>(),
                container.Resolve<GameStatsService>(),
                container.Resolve<PlayerDataProvider>(),
                container.Resolve<WalletService>(),
                container.Resolve<ConfigsProviderService>());
        }

        private static TypeModeInputService CreateTypeModeInputService(DIContainer container)
        {
            return new TypeModeInputService();
        }

        private static TypeModeCombinationGeneratorService CreateTypeModeGeneratorService(DIContainer container)
        {
            return new TypeModeCombinationGeneratorService();
        }

        private static TypeModeHandler CreateTypeModeHandler(DIContainer container)
        {
            return new TypeModeHandler(_inputArgs, 
                container.Resolve<ICoroutinesPerformer>(), 
                container.Resolve<TypeModeCombinationGeneratorService>(),
                container.Resolve<TypeModeInputService>(),
                container.Resolve<TypeModeResultService>());
        }
    }
}
