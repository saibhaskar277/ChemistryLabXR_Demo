using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedTextTMP : MonoBehaviour
{
    [SerializeField] private LocalizationKey key;

    private TextMeshProUGUI textComponent;
    private Coroutine waitCoroutine;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (waitCoroutine != null) StopCoroutine(waitCoroutine);
        waitCoroutine = StartCoroutine(WaitForManager());
    }

    private void OnDisable()
    {
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }

        if (LocalizationManager.Instance != null)
            LocalizationManager.Instance.OnLanguageChanged -= UpdateText;
    }

    private IEnumerator WaitForManager()
    {
        yield return new WaitUntil(() => LocalizationManager.Instance != null);

        // Subscribe before checking IsInitialized to avoid missing an event
        // that fires between the two lines
        LocalizationManager.Instance.OnLanguageChanged += UpdateText;

        if (LocalizationManager.Instance.IsInitialized)
            UpdateText();
    }

    private void UpdateText()
    {
        var manager = LocalizationManager.Instance;
        if (manager == null || !manager.IsInitialized) return;

        textComponent.text = manager.GetText(key);

        if (manager.CurrentFontStyle != null)
            textComponent.font = manager.CurrentFontStyle;
    }
}