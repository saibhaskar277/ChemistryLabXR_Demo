using System.Collections.Generic;
using UnityEngine;

public class MoleculeObjectiveHandler : MonoBehaviour
{
    [SerializeField] private MoleculeData targetMolecule;
    [SerializeField] private BuildMoleculeUI buildUI;

    private QuestChain currentQuest;
    private SearchAtomCombinationObjective searchObjective;
    private BuildMoleculeObjective buildObjective;

    private void Start()
    {
        CreateLesson();
        EventManager.ListenEvent<OnAtomAddedEvent>(OnAtomAddedEventListner);
    }
    
    void OnAtomAddedEventListner(OnAtomAddedEvent e)
    {
        NotifyAtomFound(e.atomType);
    }

    private void CreateLesson()
    {
        searchObjective = new SearchAtomCombinationObjective(
            $"Find atoms for {targetMolecule.moleculeType}",
            targetMolecule
        );

        buildObjective = new BuildMoleculeObjective(
            targetMolecule.moleculeType
        );

        currentQuest = new QuestChain(
            new List<BaseQuestObjective>
            {
                searchObjective,
                buildObjective
            }
        );

        buildUI.SetTargetAtomsCointaimer(targetMolecule.requirements);

        currentQuest.StartQuest();
    }

    public void NotifyAtomFound(AtomType atomType)
    {
        searchObjective?.AtomFound(atomType);
    }
}