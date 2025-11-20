using Assets._Project.Develop.Runtime.Gameplay.TypeMode;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Levels
{
    [CreateAssetMenu(menuName = "StaticData/Configs/Gameplay/Levels/New Level Config", fileName = "LevelConfig", order = 54)]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public string LevelName { get; private set; }
        [field: SerializeField] public Sprite LevelIcon { get; private set; }
        [field: SerializeField] public TypeModeSymbols TypeMode { get; private set; }
    }
}
