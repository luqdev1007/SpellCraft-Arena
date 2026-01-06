using UnityEngine;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class TowerDefenceMode : Minigame
    {
        public TowerDefenceMode()
        {
            Name = "Tower defence";
        }

        public override void Start()
        {
            Debug.Log(Name + " mode is begin!");
        }
    }
}
