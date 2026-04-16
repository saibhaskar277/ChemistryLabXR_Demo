


using System;
using UnityEngine;

public class OnMoleculeHoveredEvent: IGameEvent
{
    public MoleculeData moleculeData;

    public OnMoleculeHoveredEvent(MoleculeData data)
    {
        moleculeData = data;
    }
}

public class OnMoleculeDelectedEvent: IGameEvent
{

}

public struct OnAtomAddedEvent: IGameEvent
{
    public AtomType atomType;
    public OnAtomAddedEvent(AtomType type)
    {
        atomType = type;
    }
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



public class OnSpeechPlayEvent : IGameEvent
{
    public string text;

    public OnSpeechPlayEvent(string text)
    {
        this.text = text;
    }
}

public class OnSpeechStopEvent : IGameEvent
{
}

public class OnSpeechLocalizationKeyEvent : IGameEvent
{
    public LocalizationKey key;

    public OnSpeechLocalizationKeyEvent(LocalizationKey key)
    {
        this.key = key;
    }
}