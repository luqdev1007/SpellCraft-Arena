using Assets._Project.Develop.Runtime.Utilites.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class WinState : State, IUpdatableState
    {
        public override void Enter()
        {
            base.Enter();

            Debug.Log("win");
        }

        public void Update(float deltaTime)
        {
            
        }
    }
}