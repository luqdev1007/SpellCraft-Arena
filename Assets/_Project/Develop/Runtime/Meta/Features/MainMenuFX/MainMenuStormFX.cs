using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Features.MainMenuFX
{
    /// <summary>
    /// Ambient storm dressing for the main menu: fires lightning flashes at random intervals.
    /// A flash brightens the scene sun and the (disabled) light that feeds Shaped Clouds,
    /// so the whole overcast sky lights up, and spawns a short-lived bolt VFX in the clouds.
    /// </summary>
    public class MainMenuStormFX : MonoBehaviour
    {
        [Header("Lights")]
        [SerializeField] private Light _sceneSun;
        [Tooltip("Disabled directional light assigned as Custom Sun Light on the cloud controllers.")]
        [SerializeField] private Light _cloudsSun;

        [Header("Bolt VFX")]
        [SerializeField] private GameObject _boltVfxPrefab;
        [Tooltip("Bolts appear around this point, usually the tower top.")]
        [SerializeField] private Transform _boltAnchor;
        [SerializeField] private float _boltSpawnRadius = 110f;
        [SerializeField] private float _boltHeightSpread = 60f;
        [SerializeField] private float _boltScale = 25f;
        [SerializeField] private float _boltLifetime = 2.5f;
        [Tooltip("Recolors the spawned bolt's particles so stock VFX match the scene palette.")]
        [SerializeField] private bool _tintBolt = true;
        [SerializeField] private Color _boltTint = new Color(1f, 0.93f, 0.72f, 1f);

        [Header("Timing")]
        [SerializeField] private Vector2 _intervalRange = new Vector2(5f, 13f);
        [SerializeField] private float _sceneFlashIntensity = 4f;
        [SerializeField] private float _cloudsFlashIntensity = 3.5f;
        [SerializeField] private float _flashInDuration = 0.06f;
        [SerializeField] private float _flashOutDuration = 0.35f;

        private float _sceneSunBaseIntensity;
        private float _cloudsSunBaseIntensity;

        private void Start()
        {
            if (_sceneSun != null)
                _sceneSunBaseIntensity = _sceneSun.intensity;

            if (_cloudsSun != null)
                _cloudsSunBaseIntensity = _cloudsSun.intensity;

            StartCoroutine(StrikeLoop());
        }

        private void OnDisable()
        {
            RestoreLights();
        }

        private IEnumerator StrikeLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(_intervalRange.x, _intervalRange.y));

                Strike();

                // Distant storms rarely flash once — a second, weaker beat sells it.
                if (Random.value < 0.6f)
                {
                    yield return new WaitForSeconds(Random.Range(0.15f, 0.4f));
                    Flash(0.5f);
                }
            }
        }

        private void Strike()
        {
            Flash(1f);
            SpawnBolt();
        }

        private void Flash(float strength)
        {
            if (_sceneSun != null)
                Tween(_sceneSun, _sceneSunBaseIntensity, Mathf.Lerp(_sceneSunBaseIntensity, _sceneFlashIntensity, strength));

            if (_cloudsSun != null)
                Tween(_cloudsSun, _cloudsSunBaseIntensity, Mathf.Lerp(_cloudsSunBaseIntensity, _cloudsFlashIntensity, strength));
        }

        private void Tween(Light light, float baseIntensity, float peakIntensity)
        {
            DOTween.Kill(light);

            DOTween.Sequence()
                .SetTarget(light)
                .Append(DOTween.To(() => light.intensity, value => light.intensity = value, peakIntensity, _flashInDuration))
                .Append(DOTween.To(() => light.intensity, value => light.intensity = value, baseIntensity, _flashOutDuration)
                    .SetEase(Ease.OutQuad));
        }

        private void SpawnBolt()
        {
            if (_boltVfxPrefab == null || _boltAnchor == null)
                return;

            Vector2 offset = Random.insideUnitCircle * _boltSpawnRadius;
            Vector3 position = _boltAnchor.position
                + new Vector3(offset.x, Random.Range(-_boltHeightSpread, _boltHeightSpread) * 0.5f, offset.y);

            GameObject bolt = Instantiate(_boltVfxPrefab, position, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
            bolt.transform.localScale = Vector3.one * _boltScale;

            if (_tintBolt)
            {
                foreach (ParticleSystem particles in bolt.GetComponentsInChildren<ParticleSystem>(true))
                {
                    ParticleSystem.MainModule main = particles.main;
                    main.startColor = new ParticleSystem.MinMaxGradient(_boltTint);
                }
            }

            Destroy(bolt, _boltLifetime);
        }

        private void RestoreLights()
        {
            if (_sceneSun != null)
            {
                DOTween.Kill(_sceneSun);
                _sceneSun.intensity = _sceneSunBaseIntensity;
            }

            if (_cloudsSun != null)
            {
                DOTween.Kill(_cloudsSun);
                _cloudsSun.intensity = _cloudsSunBaseIntensity;
            }
        }
    }
}
