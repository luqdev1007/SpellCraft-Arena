using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AspectButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _testAspectsText;
    [SerializeField] private string _aspectName;

    private Button _button;

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
        _testAspectsText.text = "Last Used Aspect: " + _aspectName;
    }
}
