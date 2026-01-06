using System;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class MinigameStatesContext : IDisposable
    {
        private MinigameStateMachine _minigameStateMachine;

        private bool _isRunning;

        public MinigameStatesContext(MinigameStateMachine gameplayStateMachine)
        {
            _minigameStateMachine = gameplayStateMachine;
        }

        public void Dispose()
        {
            _isRunning = false;
            _minigameStateMachine.Dispose();
        }

        public void Run()
        {
            _minigameStateMachine.Enter();
            _isRunning = true;
        }

        public void Update(float deltaTime)
        {
            if (_isRunning == false)
                return;

            _minigameStateMachine.Update(deltaTime);
        }
    }
}
