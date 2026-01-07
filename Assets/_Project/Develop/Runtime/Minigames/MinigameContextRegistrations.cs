using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
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

            container.RegisterAsSingle<IInputService>(CreateDesktopInput);

            container.RegisterAsSingle(CreateMinigamesFactory);
            container.RegisterAsSingle(CreateMinigameStatesFactory);
            container.RegisterAsSingle(CreateMinigameCreaturesFactory);

            container.RegisterAsSingle(CreateGameplayStatesContext);

            // ui
        }

        private static MinigamesCreaturesFactory CreateMinigameCreaturesFactory(DIContainer container)
        {
            return new MinigamesCreaturesFactory(container);
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
    }
}
