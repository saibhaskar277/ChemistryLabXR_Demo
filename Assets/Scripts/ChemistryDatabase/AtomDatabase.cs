using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(menuName = "XR Chemistry/Atom Database")]
public class AtomDatabase : ScriptableObject
{
    public List<AtomViewItemData> atoms;

    public AtomViewItemData GetAtomDataByType(AtomType type)
    {
        return atoms.Find(a => a.atomType == type);
    }
}

[System.Serializable]
public class AtomViewItemData
{
    public AtomType atomType;
    public string displayName;
    public Sprite icon;
    public GameObject prefab;
}