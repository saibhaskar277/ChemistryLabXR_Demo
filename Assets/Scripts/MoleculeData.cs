using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "XR Chemistry/Molecule")]
public class MoleculeData : ScriptableObject
{
    public MoleculeType moleculeType;
    public string moleculeName;
    public string formula;
    public GameObject moleculePrefab;
    public string description;
    public string bondType;   
    public string bondAngle;
    public List<AtomRequirement> requirements;
}

[System.Serializable]
public class AtomRequirement
{
    public AtomType atomType;
    public int count;
}

public enum AtomType
{
    Hydrogen,
    Oxygen,
    Carbon,
    Nitrogen
}

public enum MoleculeType
{
    Water,
    CarbonDioxide,
    Methane,
    Ammonia,
    Hydrogen,
    Oxygen,
    Carbon,
    Nitrogen
}