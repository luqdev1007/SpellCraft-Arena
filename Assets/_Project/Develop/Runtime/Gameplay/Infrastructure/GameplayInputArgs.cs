using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Utilites.SceneManagement
{
    public class GameplayInputArgs : IInputSceneArgs
    {
        public GameplayInputArgs(string allowedSymbols)
        {
            AllowedSymbols = allowedSymbols;
        }

        public string AllowedSymbols { get; private set; }
    }
}
