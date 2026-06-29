using Assets._Project.Develop.Infrastructure;
using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    /// <summary>
    /// Dev-only MonoBehaviour. Place on any GameObject in MainMenu scene.
    /// When enabled, automatically launches Gameplay after the bootstrap finishes initialising,
    /// bypassing the level-select UI — exactly as LevelTilePresenter.OnViewClicked does.
    /// Toggle _autoStart in the Inspector to enable/disable without removing the component.
    /// </summary>
    public class DevAutoGameplayLauncher : MonoBehaviour
    {
        [SerializeField] private bool _autoStart = true;
        [SerializeField] private int _levelNumber = 1;

        private IEnumerator Start()
        {
            if (!_autoStart)
                yield break;

            // Wait for MainMenuBootstrap.Initialize() + Run() to complete (called by SceneSwitcherService).
            // Bootstrap runs in the same frame as scene load but via coroutine, so we wait a few frames.
            yield return null;
            yield return null;
            yield return null;

            MainMenuBootstrap bootstrap = FindFirstObjectByType<MainMenuBootstrap>();

            if (bootstrap == null)
            {
                Debug.LogError("[DevAutoGameplayLauncher] MainMenuBootstrap not found in scene.");
                yield break;
            }

            // Retrieve private fields via reflection — acceptable for a dev tool only used in Editor.
            DIContainer container = GetPrivateField<DIContainer>(bootstrap, "_container");
            ICoroutinesPerformer performer = GetPrivateField<ICoroutinesPerformer>(bootstrap, "_coroutinesPerformer");

            if (container == null || performer == null)
            {
                Debug.LogError("[DevAutoGameplayLauncher] Bootstrap not yet initialised. Try increasing yield count.");
                yield break;
            }

            SceneSwitcherService sceneSwitcher = container.Resolve<SceneSwitcherService>();
            GameplayInputArgs inputArgs = new GameplayInputArgs(_levelNumber, Vector3.zero);

            Debug.Log($"[DevAutoGameplayLauncher] Auto-launching Gameplay (level {_levelNumber})...");
            performer.StartPerform(sceneSwitcher.ProcessingSwitchTo(Scenes.Gameplay, inputArgs));
        }

        private static T GetPrivateField<T>(object target, string fieldName) where T : class
        {
            FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            return field?.GetValue(target) as T;
        }
    }
}
