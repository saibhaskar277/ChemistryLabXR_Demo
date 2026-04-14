using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class AtomController : XRGrabInteractable
{
    public AtomType atomType;

    private bool isInsideBondZone = false;
    private BondManager bondManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BondZone"))
        {
            isInsideBondZone = true;
            bondManager = other.GetComponent<BondZone>().bondManager;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BondZone"))
        {
            isInsideBondZone = false;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (isInsideBondZone && bondManager != null)
        {
            bondManager.AddAtom(this);
        }
    }
}