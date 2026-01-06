using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class MinigameInputArgs : IInputSceneArgs
    { 
        public MinigameInputArgs(MinigameModes mode)
        {
            Mode = mode;
        }

        public MinigameModes Mode { get; private set; }
    }
}
