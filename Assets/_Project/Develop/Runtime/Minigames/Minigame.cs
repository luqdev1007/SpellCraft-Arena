using System;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public abstract class Minigame
    {
        public abstract bool IsMinigameCompleted { get; protected set; }
        public abstract bool PreperationOver { get; protected set; }
        public abstract bool ReturnToPreperation { get; protected set; }

        public abstract void Init();

        public abstract void Update(float deltaTime);

        public abstract void StartPreperation();
    }
}
