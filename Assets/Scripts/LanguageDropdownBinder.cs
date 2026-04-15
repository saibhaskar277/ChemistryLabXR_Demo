using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageDropdownBinder : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    public Action<Language> OnLanguageSelected;

    private void Awake()
    {
        PopulateDropdown();
        dropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownChanged);
    }

    private void PopulateDropdown()
    {
        dropdown.ClearOptions();

        List<string> options = new();

        foreach (Language language in Enum.GetValues(typeof(Language)))
        {
            options.Add(language.ToString());
        }

        dropdown.AddOptions(options);
    }

    private void OnDropdownChanged(int index)
    {
        Language selectedLanguage = (Language)index;
        OnLanguageSelected?.Invoke(selectedLanguage);
        LocalizationManager.Instance.SetLanguage(selectedLanguage);
    }
}