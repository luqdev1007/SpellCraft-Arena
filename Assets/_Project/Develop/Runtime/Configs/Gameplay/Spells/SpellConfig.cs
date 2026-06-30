using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Spells
{
    [CreateAssetMenu(fileName = "SpellConfig", menuName = "Configs/Gameplay/Spells/New Spell Config")]
    public class SpellConfig : ScriptableObject
    {
        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: SerializeField] public Aspect[] Aspects { get; private set; } = new Aspect[3];

        [field: SerializeField, Min(0)] public float ManaCost { get; private set; } = 25f;
        [field: SerializeField, Min(0)] public float CastTime { get; private set; } = 0.4f;
        [field: SerializeField] public SpellCastType CastType { get; private set; }

        [field: SerializeField] public string PrefabPath { get; private set; }
        [field: SerializeField] public float Damage { get; private set; } = 50f;

        [Header("Circle AoE (legacy)")]
        [field: SerializeField] public float AoeRadius { get; private set; } = 3f;

        [Header("Cone AoE")]
        [field: SerializeField, Min(0.5f)] public float ConeRange { get; private set; } = 5f;
        [field: SerializeField, Range(10f, 180f)] public float ConeAngle { get; private set; } = 60f;
    }
}
