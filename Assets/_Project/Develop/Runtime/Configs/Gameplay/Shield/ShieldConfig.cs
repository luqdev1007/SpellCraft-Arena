using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Shield
{
    [CreateAssetMenu(fileName = "ShieldConfig", menuName = "Configs/Gameplay/Shield/New Shield Config")]
    public class ShieldConfig : ScriptableObject
    {
        [field: SerializeField, Range(0.01f, 100f)] public float AbsorbPercent { get; private set; } = 50f;
        [field: SerializeField, Min(0.01f)] public float ManaPerUnit { get; private set; } = 0.5f;
        [field: SerializeField] public bool ActiveByDefault { get; private set; } = true;

        private void OnValidate()
        {
            AbsorbPercent = Mathf.Clamp(AbsorbPercent, 0.01f, 100f);
            ManaPerUnit = Mathf.Max(ManaPerUnit, 0.01f);
        }
    }
}
