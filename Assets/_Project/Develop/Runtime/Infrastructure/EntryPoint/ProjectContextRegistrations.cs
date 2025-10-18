using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Configs.Meta.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilites.AssetsManagment;
using Assets._Project.Develop.Runtime.Utilites.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.DataManagment;
using Assets._Project.Develop.Runtime.Utilites.DataManagment.DataRepository;
using Assets._Project.Develop.Runtime.Utilites.DataManagment.KeyStorage;
using Assets._Project.Develop.Runtime.Utilites.DataManagment.Serializers;
using Assets._Project.Develop.Runtime.Utilites.DataProviders;
using Assets._Project.Develop.Runtime.Utilites.LoadingScreen;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets._Project.Develop.Infrastructure.EntryPoint
{
    public class ProjectContextRegistrations
    {
        public static void Process(DIContainer container)
        {
            container.RegisterAsSingle<ICoroutinesPerformer>(CreateCoroutinesPerformer);

            container.RegisterAsSingle(CreateConfigProviderService);

            container.RegisterAsSingle(CreateResourcesAssetsLoader);

            container.RegisterAsSingle(CreateSceneLoaderService);

            container.RegisterAsSingle(CreateSceneSwitcherService);

            container.RegisterAsSingle<ILoadingScreen>(CreateStandartLoadingScreen);

            container.RegisterAsSingle(CreateWalletService).NonLazy();

            container.RegisterAsSingle(CreatePlayerDataProvider);

            container.RegisterAsSingle<ISaveLoadService>(CreateSaveLoadService);

            container.RegisterAsSingle(CreateGameStatsService);
        }

        private static GameStatsService CreateGameStatsService(DIContainer container)
        {
            return new GameStatsService(container.Resolve<PlayerDataProvider>(), container.Resolve<WalletService>(), container.Resolve<ConfigsProviderService>());
        }

        private static PlayerDataProvider CreatePlayerDataProvider(DIContainer container)
        {
            return new PlayerDataProvider(container.Resolve<ISaveLoadService>(), container.Resolve<ConfigsProviderService>());
        }

        private static SaveLoadService CreateSaveLoadService(DIContainer container)
        {
            IDataSerializer dataSerializer = new JsonSerializer();
            IDataKeysStorage dataKeysStorage = new MapDataKeysStorage();

            string saveFolderPath = Application.isEditor? Application.dataPath : Application.persistentDataPath;

            IDataRepository dataRepository = new LocalFileDataRepository(saveFolderPath, "json");

            return new SaveLoadService(dataSerializer, dataKeysStorage, dataRepository);
        }

        private static WalletService CreateWalletService(DIContainer container)
        {
            Dictionary<CurrencyTypes, ReactiveVariable<int>> currencies = new();
            
            foreach (CurrencyTypes currencyType in Enum.GetValues(typeof(CurrencyTypes)))
            {
                currencies[currencyType] = new ReactiveVariable<int>(0);
            }

            return new WalletService(currencies, container.Resolve<PlayerDataProvider>());
        }

        private static SceneSwitcherService CreateSceneSwitcherService(DIContainer container)
        {
            return new SceneSwitcherService(container.Resolve<SceneLoaderService>(), container.Resolve<ILoadingScreen>(), container);
        }

        private static SceneLoaderService CreateSceneLoaderService(DIContainer container)
        {
            return new SceneLoaderService();
        }

        private static ConfigsProviderService CreateConfigProviderService(DIContainer container)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = container.Resolve<ResourcesAssetsLoader>();
            ResourcesConfigsLoader resourcesConfigsLoader = new ResourcesConfigsLoader(resourcesAssetsLoader);

            return new ConfigsProviderService(resourcesConfigsLoader);
        }

        private static ResourcesAssetsLoader CreateResourcesAssetsLoader(DIContainer container)
        {
            return new ResourcesAssetsLoader();
        }

        private static CoroutinesPerformer CreateCoroutinesPerformer(DIContainer container)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = container.Resolve<ResourcesAssetsLoader>();

            CoroutinesPerformer coroutinesPerformer = resourcesAssetsLoader
                .Load<CoroutinesPerformer>("Utilities/CoroutinesPerformer");

            return Object.Instantiate(coroutinesPerformer);
        }

        private static StandartLoadingScreen CreateStandartLoadingScreen(DIContainer container)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = container.Resolve<ResourcesAssetsLoader>();

            StandartLoadingScreen standartLoadingScreen = resourcesAssetsLoader
                .Load<StandartLoadingScreen>("Utilities/StandartLoadingScreen");

            return Object.Instantiate(standartLoadingScreen);
        }
    }
}