using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.TypeMode
{
    public class TypeModeCombinationGeneratorService
    {
        private readonly string[] _allowedSymbols;

        public TypeModeCombinationGeneratorService(string[] allowedSymbols)
        {
            _allowedSymbols = allowedSymbols;
        }

        public string GenerateCombination(int length = 5)
        {
            string combination = "";

            for (int i = 0; i < length; i++)
            {
                string randomSymbol = _allowedSymbols[Random.Range(0, _allowedSymbols.Length)];
                combination += randomSymbol;
            }

            return combination;
        }
    }
}