using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellDatabase", menuName = "StaticData/Gameplay/Spell Database")]
public class SpellDatabase : ScriptableObject
{
    [SerializeField] private List<SpellCombinationConfig> _combinations;

    public SpellCombinationConfig FindSpell(List<AspectNames> combination)
    {
        if (combination.Count != 3)
            return null;

        foreach (var spell in _combinations)
        {
            if (spell.MatchesCombination(combination[0], combination[1], combination[2]))
                return spell;
        }

        return null;
    }
}
