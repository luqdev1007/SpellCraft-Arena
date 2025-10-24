using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Wallet;
using Assets._Project.Develop.Runtime.Utilites.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.DataProviders;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuContextRegistrations
    {
        public static void Process(DIContainer container)
        {
            Debug.Log("Process registrations on main menu scene");

            container.RegisterAsSingle(CreateGameModeSelectionService);
        }

        private static GameModeSelectionService CreateGameModeSelectionService(DIContainer container)
        {
            return new GameModeSelectionService(
                   container.Resolve<SceneSwitcherService>(),
                   container.Resolve<ICoroutinesPerformer>(),
                   container.Resolve<ConfigsProviderService>()
               );
        }
    }
}
