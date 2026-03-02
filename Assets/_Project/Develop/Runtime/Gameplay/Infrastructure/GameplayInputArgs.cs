using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Utilites.SceneManagement
{
    public class GameplayInputArgs : IInputSceneArgs
    {
        public GameplayInputArgs(string allowedSymbols, int levelNumber, Vector3 levelSpawnPointPosition)
        {
            AllowedSymbols = allowedSymbols;
            LevelNumber = levelNumber;
            LevelSpawnPointPosition = levelSpawnPointPosition;
        }

        public string AllowedSymbols { get; private set; }
        public int LevelNumber { get; private set; }
        public Vector3 LevelSpawnPointPosition { get; private set; }
    }
}
