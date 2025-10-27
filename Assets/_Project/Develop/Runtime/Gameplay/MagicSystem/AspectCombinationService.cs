using System.Collections.Generic;
using UnityEngine;

public class AspectCombinationHandler : MonoBehaviour
{
    private const int MAX_COMBINATION_SIZE = 3;

    [SerializeField] private SpellDatabase _spellDatabase;
    [SerializeField] private MagicCastBarView _view;

    private readonly List<AspectNames> _currentCombination = new();

    private SpellCombinationConfig _currentActiveSpell;

    private void OnEnable()
    {
        AspectButton.Clicked += OnAspectClicked;
        MagicCastBarView.AspectsPanelToggled += OnAspectsPanelToggled;
        MagicCastBarView.AttackButtonClicked += OnAttackButtonClicked;
    }

    private void OnAttackButtonClicked()
    {
        if (_currentActiveSpell == null)
            return;

        Debug.Log("Cast: " + _currentActiveSpell.SpellName);
    }

    private void OnDisable()
    {
        AspectButton.Clicked -= OnAspectClicked;
        MagicCastBarView.AspectsPanelToggled -= OnAspectsPanelToggled;
        MagicCastBarView.AttackButtonClicked -= OnAttackButtonClicked;
    }

    private void OnAspectsPanelToggled(bool state)
    {
        if (state)
            OnPanelOpened();
        else
            OnPanelClosed();
    }

    private void OnAspectClicked(AspectNames aspect)
    {
        if (_currentCombination.Contains(aspect))
            return;

        if (_currentCombination.Count >= MAX_COMBINATION_SIZE)
            _currentCombination.RemoveAt(0);

        _currentCombination.Add(aspect);

        Debug.Log($"Комбинация: {string.Join(", ", _currentCombination)}");
    }

    private void OnPanelOpened()
    {
        _currentCombination.Clear();
        Debug.Log("Панель аспектов открыта, новая комбинация начинается.");
    }

    private void OnPanelClosed()
    {
        if (_currentCombination.Count < MAX_COMBINATION_SIZE)
            return;

        SpellCombinationConfig spell = _spellDatabase.FindSpell(_currentCombination);

        if (spell != null)
        {
            Debug.Log($"✨ Выбрано заклинание: {spell.SpellName}");
            _currentActiveSpell = spell;
            _view.CurrentActiveSpellImage.sprite = spell.Icon;
        }
        else
        {
            Debug.Log("❌ Нет заклинания с такой комбинацией аспектов.");
        }

        _currentCombination.Clear();
    }
}
