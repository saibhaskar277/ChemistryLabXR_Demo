using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AtomViewItem : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text label;

    private AtomType atomType;
    private Action<AtomType> onClick;

    public void Bind(AtomViewItemData data, Action<AtomType> clickAction)
    {
        atomType = data.atomType;
        onClick = clickAction;

        label.text = data.displayName;
        if (icon != null) icon.sprite = data.icon;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnPressed);
    }

    private void OnPressed()
    {
        onClick?.Invoke(atomType);
    }
}
