using System;
using TMPro;
using UnityEngine;

public class TargetAtomView : MonoBehaviour
{
    public AtomType targetAtomType { get; private set; }
    [SerializeField] TextMeshProUGUI targetAtomText;
    [SerializeField] TextMeshProUGUI atomCount;

    public void SetTargetAtomData(string atomName, int count)
    {
        targetAtomText.text = atomName;
        atomCount.text = count.ToString();
        targetAtomType = (AtomType)Enum.Parse(typeof(AtomType), atomName);
    }

    public void UpdateTargetCount(int count)
    {
        atomCount.text = count.ToString();
    }
}
