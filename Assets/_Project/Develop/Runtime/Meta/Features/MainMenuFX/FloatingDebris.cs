using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Features.MainMenuFX
{
    /// <summary>
    /// Slow bob and spin for the rock shards levitating around the tower in the main menu.
    /// Phase is randomised per instance so a group of shards never moves in lockstep.
    /// </summary>
    public class FloatingDebris : MonoBehaviour
    {
        [SerializeField] private float _bobAmplitude = 1.5f;
        [SerializeField] private float _bobSpeed = 0.35f;
        [SerializeField] private Vector3 _spinSpeed = new Vector3(3f, 8f, 2f);

        private Vector3 _origin;
        private float _phase;

        private void Awake()
        {
            _origin = transform.position;
            _phase = Random.Range(0f, Mathf.PI * 2f);
        }

        private void Update()
        {
            float offset = Mathf.Sin(Time.time * _bobSpeed * Mathf.PI * 2f + _phase) * _bobAmplitude;
            transform.position = _origin + Vector3.up * offset;
            transform.Rotate(_spinSpeed * Time.deltaTime, Space.Self);
        }
    }
}
