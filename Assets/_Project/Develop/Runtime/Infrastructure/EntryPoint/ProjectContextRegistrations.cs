using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilites.AssetsManagment;
using Assets._Project.Develop.Runtime.Utilites.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.LoadingScreen;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using UnityEngine;

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