using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.TypeMode
{
    public class TypeModeConfig : ScriptableObject
    {
        [field: SerializeField] public string Symbols { get; private set; }
    }
}