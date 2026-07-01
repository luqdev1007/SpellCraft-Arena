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

        [Tooltip("Moment when the spell fires, in seconds relative to a base 1-second animation. " +
                 "Scaled by the same speed multiplier as the animation. Default 0.6 = frame at 60% of the swing.")]
        [field: SerializeField, Min(0)] public float CastMomentTime { get; private set; } = 0.6f;
        [field: SerializeField] public SpellCastType CastType { get; private set; }

        [field: SerializeField] public string PrefabPath { get; private set; }
        [field: SerializeField] public float Damage { get; private set; } = 50f;

        [Header("Circle AoE (legacy)")]
        [field: SerializeField] public float AoeRadius { get; private set; } = 3f;

        [Header("Cone AoE")]
        [field: SerializeField, Min(0.5f)] public float ConeRange { get; private set; } = 5f;
        [field: SerializeField, Range(10f, 180f)] public float ConeAngle { get; private set; } = 60f;

        [Header("Chain Lightning")]
        [field: SerializeField, Min(0.05f)] public float HitscanRadius { get; private set; } = 0.5f;
        [field: SerializeField, Min(1f)] public float HitscanRange { get; private set; } = 20f;
        [field: SerializeField, Min(0)] public int BounceCount { get; private set; } = 3;
        [field: SerializeField, Min(0.1f)] public float BounceRange { get; private set; } = 6f;
        [field: SerializeField, Range(0f, 1f)] public float DamageFalloff { get; private set; } = 0.75f;
        [field: SerializeField, Min(0)] public float JumpDelay { get; private set; } = 0.15f;
        [field: SerializeField] public bool CanHitSameTarget { get; private set; } = false;
        [field: SerializeField, Min(0.1f)] public float BeamDisplayDuration { get; private set; } = 0.6f;
    }
}
