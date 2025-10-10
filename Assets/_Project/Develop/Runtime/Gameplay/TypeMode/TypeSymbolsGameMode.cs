using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.TypeMode
{
    public class TypeSymbolsGameMode : ScriptableObject
    {
        [field: SerializeField] public string[] Symbols { get; private set; }
    }
}