using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicCastBarView : MonoBehaviour
{
    [SerializeField] private Button _openAspectsButton;
    [SerializeField] private AspectButton[] _aspectButtons;

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

    private IEnumerator Toggling()
    {
        float openningDelay = 0.025f;
        WaitForSeconds delay = new WaitForSeconds(openningDelay);

        if (_openned)
        {
            for (int i = _aspectButtons.Length - 1; i >= 0; i--)
            {
                _aspectButtons[i].gameObject.SetActive(false);

                yield return delay;
            }

            _openned = false;
        }
        else
        {
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
