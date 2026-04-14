using UnityEngine;

public class BondZone : MonoBehaviour
{
    public BondManager bondManager;

    private void OnTriggerEnter(Collider other)
    {
        AtomController atom = other.GetComponentInParent<AtomController>();

        if (atom != null)
        {
            bondManager.AddAtom(atom);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        AtomController atom = other.GetComponentInParent<AtomController>();

        if (atom != null)
        {
            bondManager.RemoveAtom(atom);
        }
    }
}