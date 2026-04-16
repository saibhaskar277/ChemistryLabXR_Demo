using UnityEngine;

public class BuildMoleculeObjective : BaseQuestObjective
{
    private readonly MoleculeType target;

    public BuildMoleculeObjective(MoleculeType target)
        : base($"Build {target}")
    {
        this.target = target;
    }

    public override void StartObjective()
    {
        base.StartObjective();
        EventManager.ListenEvent<OnMoleculeHoveredEvent>(Check);
    }

    private void Check(OnMoleculeHoveredEvent e)
    {
        if (e.moleculeData.moleculeType == target)
        {
            EventManager.StopListening<OnMoleculeHoveredEvent>(Check);
            CompleteObjective();
        }
    }
}