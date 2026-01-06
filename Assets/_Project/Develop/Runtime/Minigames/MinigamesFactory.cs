using System;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class MinigamesFactory
    {
        public Minigame CreateBy(MinigameModes mode)
        {
            switch (mode)
            {
                case MinigameModes.TowerDefence:
                    return new TowerDefenceMode();

                default:
                    throw new ArgumentException($"{mode} mode is not determined");
            }
        }
    }
}
