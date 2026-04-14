


using System;
using UnityEngine;

public class OnMoleculeCreatedEvent: IGameEvent
{
    public MoleculeData moleculeData;

    public OnMoleculeCreatedEvent(MoleculeData data, GameObject obj)
    {
        moleculeData = data;
    }
}

public class OnMoleculeDelectedEvent: IGameEvent
{

}

public class OnMoleculeInfoRequestedEvent: IGameEvent
{
    public MoleculeType moleculeType;   
    public Action<MoleculeData> moleculeData;

    public OnMoleculeInfoRequestedEvent(MoleculeType type, Action<MoleculeData> data)
    {
        moleculeType = type;
        moleculeData = data;
    }
}