using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilites.Conditions;

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
            Minigame minigame = _container.Resolve<MinigamesFactory>().CreateFor(inputArgs.Mode);

            MinigameStateMachine coreLoop = new MinigameStateMachine();

            InitState initState = new InitState(minigame);
            PreperationState preperationState = new PreperationState(minigame);
            UpdateState updateState = new UpdateState(minigame);
            WinState winState = new WinState();
                
            coreLoop.AddState(initState);
            coreLoop.AddState(preperationState);
            coreLoop.AddState(updateState);
            coreLoop.AddState(winState);

            ICompositeCondition fromInitStateToPreperationState = new CompositeCondition()
                .Add(new FuncCondition(() => minigame.ReturnToPreperation));

            ICompositeCondition fromPreperationStateToUpdateState = new CompositeCondition()
                .Add(new FuncCondition(() => minigame.PreperationOver));

            ICompositeCondition fromUpdateStateToPreperationState = new CompositeCondition()
                .Add(new FuncCondition(() => minigame.ReturnToPreperation));

            ICompositeCondition fromUpdateStateToWinState = new CompositeCondition()
                .Add(new FuncCondition(() => minigame.IsMinigameCompleted));

            coreLoop.AddTransition(initState, preperationState, fromInitStateToPreperationState);
            coreLoop.AddTransition(preperationState, updateState, fromPreperationStateToUpdateState);
            coreLoop.AddTransition(updateState, preperationState, fromUpdateStateToPreperationState);
            coreLoop.AddTransition(updateState, winState, fromUpdateStateToWinState);

            return coreLoop;
        }
    }
}
