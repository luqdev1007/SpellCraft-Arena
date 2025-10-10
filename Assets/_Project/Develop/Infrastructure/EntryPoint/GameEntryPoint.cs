﻿using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilites.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.LoadingScreen;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Infrastructure.EntryPoint
{
    public class GameEntryPoint : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("Start project, setup settings");

            SetupAppSettings();

            Debug.Log("Global process registations for project");

            DIContainer projectContainer = new DIContainer();
            ProjectContextRegistrations.Process(projectContainer);

            ICoroutinesPerformer coroutinePerformer = projectContainer.Resolve<ICoroutinesPerformer>();
            coroutinePerformer.StartPerform(Initialize(projectContainer));
        }

        private void SetupAppSettings()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        private IEnumerator Initialize(DIContainer container)
        {
            ILoadingScreen loadingScreen = container.Resolve<ILoadingScreen>();
            SceneSwitcherService sceneSwitcherService = container.Resolve<SceneSwitcherService>();

            loadingScreen.Show();

            Debug.Log("Begin servises init...");

            yield return container.Resolve<ConfigsProviderService>().LoadAsync();

            yield return new WaitForSeconds(1); // simulation of long inits

            Debug.Log("Servises init is finished");

            loadingScreen.Hide();

            yield return sceneSwitcherService.ProcessingSwitchTo(Scenes.MainMenu);
        }
    }
}