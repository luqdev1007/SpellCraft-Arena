using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AspectButton : MonoBehaviour
{
    [SerializeField] private AspectNames _aspectName;

    private Button _button;

    public static event Action<AspectNames> Clicked;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        Clicked?.Invoke(_aspectName);
    }
}
