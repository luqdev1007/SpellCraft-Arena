using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Utilites.SceneManagement
{
    [Serializable]
    public class GameplayInputArgs : IInputSceneArgs
    {
        [field:SerializeField] public string AllowedSymbols { get; private set; }
        [field: SerializeField] public string LevelName { get; private set; }
        [field: SerializeField] public Sprite LevelIcon { get; private set; }
    }
}
