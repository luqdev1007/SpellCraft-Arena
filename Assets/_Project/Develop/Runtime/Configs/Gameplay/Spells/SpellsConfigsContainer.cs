using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Spells
{
    [CreateAssetMenu(fileName = "SpellsConfigsContainer", menuName = "Configs/Gameplay/Spells/New Spells Container")]
    public class SpellsConfigsContainer : ScriptableObject
    {
        [field: SerializeField] public List<SpellConfig> Spells { get; private set; } = new();

        [Tooltip("Icons in Aspect enum order: Blood, Fire, Light, Death, Nature, Ice, Magic")]
        [field: SerializeField] public Sprite[] AspectIcons { get; private set; } = new Sprite[7];

        [Tooltip("Spell shown and active immediately on game start, before the player picks a combo")]
        [field: SerializeField] public SpellConfig DefaultSpell { get; private set; }

        public SpellConfig FindByAspects(IReadOnlyList<Aspect> aspects)
        {
            if (aspects == null || aspects.Count != 3)
                return null;

            foreach (SpellConfig spell in Spells)
            {
                if (spell.Aspects == null || spell.Aspects.Length != 3)
                    continue;

                if (IsAspectMatch(spell.Aspects, aspects))
                    return spell;
            }

            return null;
        }

        private static bool IsAspectMatch(Aspect[] spellAspects, IReadOnlyList<Aspect> selected)
        {
            var pool = new List<Aspect>(spellAspects);

            foreach (Aspect a in selected)
            {
                if (!pool.Remove(a))
                    return false;
            }

            return pool.Count == 0;
        }
    }
}
