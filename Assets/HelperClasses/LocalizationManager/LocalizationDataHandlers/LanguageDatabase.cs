using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LanguageDatabase", menuName = "Localization/Language Database")]
public class LanguageDatabase : ScriptableObject
{
    public List<LocalizationDataSO> languages = new();
}