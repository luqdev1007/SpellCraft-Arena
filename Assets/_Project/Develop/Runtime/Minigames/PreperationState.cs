using Assets._Project.Develop.Runtime.Utilites.StateMachineCore;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class PreperationState : State, IUpdatableState
    {
        private readonly Minigame _minigame;

        public PreperationState(Minigame minigame)
        {
            _minigame = minigame;
        }

        public override void Enter()
        {
            base.Enter();
            _minigame.Start();
        }

        public void Update(float deltaTime)
        {

        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}