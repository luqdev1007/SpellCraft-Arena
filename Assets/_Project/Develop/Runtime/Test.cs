using Assets._Project.Develop.Runtime.Utilites.AssetsManagment;
using Assets._Project.Develop.Runtime.Utilites.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime
{
    public class Test : MonoBehaviour
    {
        private ICoroutinesPerformer _coroutinesPerformer;
        private ResourcesAssetsLoader _resourcesAssetsLoader;
        private ConfigsProviderService _configProviderService;

        private void Awake()
        {
            _resourcesAssetsLoader = CreateResourcesAssetsLoader();
            _coroutinesPerformer = CreateCoroutinesPerformer();
            _configProviderService = CreateConfigProviderService();


            _coroutinesPerformer.StartPerform(LoadingConfigs());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                var config = _configProviderService.GetConfig<TestConfig>();
                Debug.Log(config.Message);
            }
        }

        private ConfigsProviderService CreateConfigProviderService()
        {
            ResourcesConfigsLoader resourcesConfigsLoader = new ResourcesConfigsLoader(_resourcesAssetsLoader);

            return new ConfigsProviderService(resourcesConfigsLoader);
        }

        private ResourcesAssetsLoader CreateResourcesAssetsLoader()
        {
            return new ResourcesAssetsLoader();
        }

        private CoroutinesPerformer CreateCoroutinesPerformer()
        {
            CoroutinesPerformer coroutinesPerformer = _resourcesAssetsLoader
                .Load<CoroutinesPerformer>("Utilities/CoroutinesPerformer");

            return Instantiate(coroutinesPerformer);
        }

        private IEnumerator LoadingConfigs()
        {
            Debug.Log("Start loading configs");
            yield return _configProviderService.LoadAsync();
            Debug.Log("End loading configs");

        }
    }
}