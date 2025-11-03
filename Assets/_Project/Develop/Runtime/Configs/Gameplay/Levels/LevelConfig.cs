using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Levels
{
    [CreateAssetMenu(menuName = "StaticData/Configs/Gameplay/Levels/New Level Config", fileName = "LevelConfig", order = 54)]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public GameplayInputArgs InputArgs { get; private set; }
    }
}
