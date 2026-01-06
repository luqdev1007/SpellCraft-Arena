using Assets._Project.Develop.Infrastructure.DI;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class MinigameStatesFactory
    {
        private readonly DIContainer _container;

        public MinigameStatesFactory(DIContainer container)
        {
            _container = container;
        }

        public MinigameStateMachine CreateMinigameStateMachine(MinigameInputArgs inputArgs)
        {
            Minigame minigame = _container.Resolve<MinigamesFactory>().CreateBy(inputArgs.Mode);

            MinigameStateMachine coreLoop = new MinigameStateMachine();

            PreperationState preperationState = CreatePreperationState(minigame);
                
            coreLoop.AddState(preperationState);

            return coreLoop;
        }

        private PreperationState CreatePreperationState(Minigame minigame)
        {
            return new PreperationState(minigame);
        }
    }
}
