using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MagicCastBarView : MonoBehaviour
{
    [SerializeField] private Button _openAspectsButton;
    [SerializeField] private AspectButton[] _aspectButtons;
    [SerializeField] private Image _aspectsArcFilledImage;

    private Coroutine _openningCoroutine;
    private bool _openned = false;

    private void OnEnable()
    {
        _openAspectsButton.onClick.AddListener(OnOpenAspectsButtonClicked);
    }

    private void OnDisable()
    {
        _openAspectsButton.onClick.RemoveListener(OnOpenAspectsButtonClicked);
    }

    private void OnOpenAspectsButtonClicked()
    {
        if (_openningCoroutine != null)
            return;

        _openningCoroutine = StartCoroutine(Toggling());
    }

    private IEnumerator OpenningArc(float duration, bool isReversed)
    {
        float time = 0f;
        float start = isReversed ? 1f : 0f;
        float end = isReversed ? 0f : 1f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            _aspectsArcFilledImage.fillAmount = Mathf.Lerp(start, end, t);

            yield return null;
        }

        _aspectsArcFilledImage.fillAmount = end;
    }

    private IEnumerator Toggling()
    {
        float openningDelay = 0.025f;
        float totalDuration = _aspectButtons.Length * openningDelay;
        WaitForSeconds delay = new WaitForSeconds(openningDelay);

        if (_openned)
        {
            StartCoroutine(OpenningArc(totalDuration, true));

            for (int i = _aspectButtons.Length - 1; i >= 0; i--)
            {
                _aspectButtons[i].gameObject.SetActive(false);
                yield return delay;
            }

            _openned = false;
        }
        else
        {
            StartCoroutine(OpenningArc(totalDuration, false));

            for (int i = 0; i < _aspectButtons.Length; i++)
            {
                _aspectButtons[i].gameObject.SetActive(true);
                yield return delay;
            }

            _openned = true;
        }

        _openningCoroutine = null;
    }
}
