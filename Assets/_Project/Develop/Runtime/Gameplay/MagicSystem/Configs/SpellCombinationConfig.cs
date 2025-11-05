using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpellCombinationConfig", menuName = "StaticData/Gameplay/New Spell Combination")]
public class SpellCombinationConfig : ScriptableObject
{
    [Header("Комбинация аспектов (3 уникальных)")]
    [field: SerializeField] public AspectNames FirstAspect { get; private set; }
    [field: SerializeField] public AspectNames SecondAspect { get; private set; }
    [field: SerializeField] public AspectNames ThirdAspect { get; private set; }

    [Header("Информация о заклинании")]
    [field: SerializeField] public string SpellName { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField, TextArea(2, 5)] public string Description { get; private set; }
    [field: SerializeField] public GameObject SpellPrefab { get; private set; }


    private void OnValidate()
    {
        if (FirstAspect == SecondAspect ||
            FirstAspect == ThirdAspect ||
            SecondAspect == ThirdAspect)
        {
            Debug.LogWarning($"⚠️ В комбинации '{name}' есть повторяющиеся аспекты — исправь!");
        }
    }

    public bool MatchesCombination(AspectNames a, AspectNames b, AspectNames c)
    {
        AspectNames[] combo = new[] { FirstAspect, SecondAspect, ThirdAspect };
        AspectNames[] test = new[] { a, b, c };

        Array.Sort(combo);
        Array.Sort(test);

        for (int i = 0; i < 3; i++)
            if (combo[i] != test[i])
                return false;

        return true;
    }
}
