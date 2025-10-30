using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.TypeMode
{
    public class TypeModeCombinationGeneratorService
    {
        public string GenerateCombination(string allowedSymbols, int length = 5)
        {
            if (allowedSymbols.Length == 0)
                return "12345";

            string combination = "";

            for (int i = 0; i < length; i++)
            {
                char randomSymbol = allowedSymbols[Random.Range(0, allowedSymbols.Length)];
                combination += randomSymbol;
            }

            return combination;
        }
    }
}