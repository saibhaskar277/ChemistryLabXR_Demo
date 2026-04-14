using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "XR Chemistry/Molecule Database")]
public class MoleculeDatabase : ScriptableObject
{
    [SerializeField] private List<MoleculeData> molecules;

    public MoleculeData GetMoleculeByType(MoleculeType type)
    {
        return molecules.Find(m => m.moleculeType == type);
    }

    public List<MoleculeData> GetAllMolecules()
    {
        return molecules;
    }
}