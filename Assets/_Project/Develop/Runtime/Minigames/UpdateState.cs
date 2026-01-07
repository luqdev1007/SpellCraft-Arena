using Assets._Project.Develop.Runtime.Utilites.StateMachineCore;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class UpdateState : State, IUpdatableState
    {
        private readonly Minigame _minigame;

        public UpdateState(Minigame minigame)
        {
            _minigame = minigame;
        }

        public void Update(float deltaTime)
        {
            _minigame.Update(deltaTime);
        }
    }
}