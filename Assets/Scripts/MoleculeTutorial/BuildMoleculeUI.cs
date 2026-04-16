using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildMoleculeUI : MonoBehaviour
{
    [SerializeField] Transform atomsCointainer;


    [SerializeField] GameObject TargetAtomViewPrefab;

    [SerializeField] private TMP_Text descriptionText;
    private readonly Dictionary<AtomType, int> remainingAtoms = new();


    public void SetTargetAtomsCointaimer(List<AtomRequirement> atomRequirements)
    {
        atomRequirements.ForEach(req =>
        {
            TargetAtomView targetAtomView = Instantiate(TargetAtomViewPrefab, atomsCointainer).GetComponent<TargetAtomView>();
            targetAtomView.SetTargetAtomData(req.atomType.ToString(), req.count);
            remainingAtoms[req.atomType] = req.count;
        });
        
    }


    private void Awake()
    {
        EventManager.ListenEvent<OnAtomAddedEvent>(OnAtomAddedEvent);
    }

    private void OnAtomAddedEvent(OnAtomAddedEvent evt)
    {
        OnAtomAdded(evt.atomType);
    }

    public void OnAtomAdded(AtomType atom)
    {
        if (!remainingAtoms.ContainsKey(atom))
            return;

        if (remainingAtoms[atom] <= 0)
            return;

        remainingAtoms[atom]--;
        RefreshDescription();
    }

    private void RefreshDescription()
    {

        List<string> parts = new();

        foreach (var pair in remainingAtoms)
        {
            if (pair.Value > 0)
                parts.Add($"{pair.Value} {pair.Key}");
        }

        if (parts.Count == 0)
            descriptionText.text = $"✅ molecule completed!";
        else
            descriptionText.text = "Remaining: " + string.Join(", ", parts);
    }
}
