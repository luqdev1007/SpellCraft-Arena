using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Blink
{
    [CreateAssetMenu(fileName = "BlinkConfig", menuName = "Configs/Gameplay/Blink/New Blink Config")]
    public class BlinkConfig : ScriptableObject
    {
        [field: SerializeField, Min(0)] public float ManaCost { get; private set; } = 15f;
        [field: SerializeField, Min(0)] public float Distance { get; private set; } = 4.5f;
        [field: SerializeField, Min(0)] public float CooldownDuration { get; private set; } = 2f;
        [field: SerializeField, Min(0.05f)] public float SphereCastRadius { get; private set; } = 0.45f;
        [field: SerializeField, Min(0)] public float WallBuffer { get; private set; } = 0.1f;
        [field: SerializeField] public string VfxPrefabPath { get; private set; } = "Prefabs/Blink/Teleport";
        [field: SerializeField, Min(0)] public float VfxLifetime { get; private set; } = 4f;
    }
}
