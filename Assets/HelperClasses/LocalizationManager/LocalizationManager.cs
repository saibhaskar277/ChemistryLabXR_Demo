using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    [SerializeField] private LanguageDatabase languageDatabase;

    public bool IsInitialized { get; private set; }
    public TMP_FontAsset CurrentFontStyle { get; private set; }

    [SerializeField] private Language currentLanguage;
    public Language CurrentLanguage => currentLanguage;

    private readonly Dictionary<LocalizationKey, string> localizedText = new();
    private readonly Dictionary<LocalizationKey, string> fallbackText = new();

    private Language? fallbackLanguage = null;

    public event Action OnLanguageChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        var data = GetLanguageSO(currentLanguage);
        LoadLanguage(data);
    }

    public void SetLanguage(Language language)
    {
        if (language == currentLanguage && IsInitialized)
            return;

        var data = GetLanguageSO(language);

        if (data == null)
        {
            Debug.LogError($"[Localization] No LocalizationDataSO found for: {language}");
            return;
        }

        currentLanguage = language;
        LoadLanguage(data);
    }

    public void SetFallbackLanguage(Language language)
    {
        fallbackLanguage = language;
        fallbackText.Clear();

        var data = GetLanguageSO(language);

        if (data == null)
        {
            Debug.LogWarning($"[Localization] Fallback language '{language}' not found.");
            return;
        }

        foreach (var entry in data.entries)
            fallbackText[entry.key] = entry.value;
    }

    private LocalizationDataSO GetLanguageSO(Language language)
    {
        if (languageDatabase == null)
        {
            Debug.LogError("[Localization] LanguageDatabaseSO is missing.");
            return null;
        }

        return languageDatabase.languages.Find(x => x.languageName == language);
    }

    public void LoadLanguage(LocalizationDataSO data)
    {
        if (data == null)
        {
            Debug.LogError("[Localization] LocalizationDataSO is null!");
            return;
        }

        IsInitialized = false;
        localizedText.Clear();

        foreach (var entry in data.entries)
        {
            if (string.IsNullOrEmpty(entry.value))
                Debug.LogWarning($"[Localization] Empty value for key '{entry.key}' in '{data.languageName}'.");

            localizedText[entry.key] = entry.value;
        }

        if (data.font != null)
            CurrentFontStyle = data.font;
        else
            Debug.LogWarning($"[Localization] No font assigned in '{data.languageName}'.");

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        foreach (LocalizationKey key in Enum.GetValues(typeof(LocalizationKey)))
        {
            if (!localizedText.ContainsKey(key))
                Debug.LogError($"[Localization] Missing key in '{data.languageName}': {key}");
        }
#endif

        IsInitialized = true;
        OnLanguageChanged?.Invoke();
    }

    public string GetText(LocalizationKey key)
    {
        if (localizedText.TryGetValue(key, out string value))
            return value;

        if (fallbackLanguage.HasValue && fallbackText.TryGetValue(key, out string fallback))
        {
            Debug.LogWarning($"[Localization] Missing '{key}' in '{currentLanguage}', using fallback.");
            return fallback;
        }

        Debug.LogError($"[Localization] Missing key in all languages: {key}");
        return $"[{key}]";
    }
}