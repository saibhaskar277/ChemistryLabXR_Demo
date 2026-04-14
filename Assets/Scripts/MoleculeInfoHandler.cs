using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


[RequireComponent(typeof(XRGrabInteractable))]
public class MoleculeInfoHandler : MonoBehaviour
{
    [SerializeField] private MoleculeType moleculeType;

    XRGrabInteractable grabInteractable;
    MoleculeData moleculeData;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        EventManager.RaiseEvent( new OnMoleculeInfoRequestedEvent(moleculeType, data => moleculeData = data) );
    }



    private void OnEnable()
    {
        grabInteractable.hoverEntered.AddListener(OnHoverEntered);
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.hoverExited.AddListener(OnHoverExited);
        grabInteractable.selectExited.AddListener(OnSelectExited);   
    }

    private void OnDisable()
    {
        grabInteractable.hoverEntered.RemoveListener(OnHoverEntered);
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.hoverExited.RemoveListener(OnHoverExited);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        EventManager.RaiseEvent(new OnMoleculeCreatedEvent(moleculeData, gameObject));
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        EventManager.RaiseEvent(new OnMoleculeDelectedEvent());
    }

    void OnHoverEntered(HoverEnterEventArgs args)
    {
        if(MoleculeUI.Instance.canShowMoleculeInfo)
            EventManager.RaiseEvent(new OnMoleculeCreatedEvent(moleculeData, gameObject));
    }

    void OnHoverExited(HoverExitEventArgs args)
    {
        EventManager.RaiseEvent(new OnMoleculeDelectedEvent());
    }
}
