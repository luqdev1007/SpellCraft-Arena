using Assets._Project.Develop.Runtime.Utilites.StateMachineCore;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class InitState : State, IUpdatableState
    {
        private readonly Minigame _minigame;

        public InitState(Minigame minigame)
        {
            _minigame = minigame;
        }

        public override void Enter()
        {
            base.Enter();
            _minigame.Init();
        }

        public void Update(float deltaTime)
        {

        }
    }
}