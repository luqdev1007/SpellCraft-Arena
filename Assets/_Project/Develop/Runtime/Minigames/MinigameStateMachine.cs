using Assets._Project.Develop.Runtime.Utilites.StateMachineCore;
using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class MinigameStateMachine : StateMachine<IUpdatableState>
    {
        public MinigameStateMachine(List<IDisposable> disposables) : base(disposables)
        {
        }

        public MinigameStateMachine() : base(new List<IDisposable>())
        {
        }

        protected override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);

            CurrentState?.Update(deltaTime);
        }
    }
}
