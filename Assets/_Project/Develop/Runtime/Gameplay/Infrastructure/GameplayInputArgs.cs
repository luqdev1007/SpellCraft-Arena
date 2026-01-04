using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Utilites.SceneManagement
{
    public class GameplayInputArgs : IInputSceneArgs
    {
        public GameplayInputArgs(string allowedSymbols, int levelNumber)
        {
            AllowedSymbols = allowedSymbols;
            LevelNumber = levelNumber;
        }

        public string AllowedSymbols { get; private set; }
        public int LevelNumber { get; private set; }
    }
}
