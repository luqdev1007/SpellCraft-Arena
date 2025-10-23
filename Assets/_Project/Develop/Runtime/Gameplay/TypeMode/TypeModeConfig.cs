using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.TypeMode
{
    [CreateAssetMenu(menuName = "StaticData/Configs/Type Mode/New Type Mode Config", fileName = "TypeModeConfig", order = 54)]
    public class TypeModeConfig : ScriptableObject
    {
        [SerializeField] private List<AllowedSymbolsMap> _values;

        public string GetValueFor(TypeModeSymbols typeMode)
            => _values.First(config => config.Type == typeMode).AllowedValues;

        [Serializable]
        private class AllowedSymbolsMap
        {
            [field: SerializeField] public TypeModeSymbols Type { get; private set; }
            [field: SerializeField] public string AllowedValues { get; private set; }
        }
    }
}