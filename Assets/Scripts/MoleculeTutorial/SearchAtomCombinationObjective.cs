using System.Collections.Generic;
using UnityEngine;

public class SearchAtomCombinationObjective : BaseQuestObjective
{
    public MoleculeData TargetCombination { get; private set; }

    private readonly Dictionary<AtomType, int> foundAtoms = new();

    public SearchAtomCombinationObjective(
        string description,
        MoleculeData targetCombination
    ) : base(description)
    {
        TargetCombination = targetCombination;
    }

    public void AtomFound(AtomType atomType)
    {
        var requirement = TargetCombination.requirements
            .Find(x => x.atomType == atomType);

        if (requirement == null)
        {

            return;
        }


        if (!foundAtoms.ContainsKey(atomType))
            foundAtoms[atomType] = 0;

        if (foundAtoms[atomType] >= requirement.count) return;

        foundAtoms[atomType]++;

        CheckCompletion();
    }

    private void CheckCompletion()
    {
        foreach (var req in TargetCombination.requirements)
        {
            if (!foundAtoms.ContainsKey(req.atomType)) return;
            if (foundAtoms[req.atomType] < req.count) return;
        }

        CompleteObjective();
    }

    public override void CompleteObjective()
    {
        base.CompleteObjective();
        Debug.Log("Correct atom combination found!");
    }
}