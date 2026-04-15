using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class MoleculeInfoHandler : MonoBehaviour
{
    [SerializeField] private MoleculeType moleculeType;

    private XRGrabInteractable grabInteractable;
    private MoleculeData moleculeData;

    private bool isHovered;
    private bool isSelected;
    private bool isShowing;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        EventManager.RaiseEvent(
            new OnMoleculeInfoRequestedEvent(
                moleculeType,
                data => moleculeData = data
            )
        );
    }

    private void OnEnable()
    {
        grabInteractable.hoverEntered.AddListener(OnHoverEntered);
        grabInteractable.hoverExited.AddListener(OnHoverExited);
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void OnDisable()
    {
        grabInteractable.hoverEntered.RemoveListener(OnHoverEntered);
        grabInteractable.hoverExited.RemoveListener(OnHoverExited);
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    private void RefreshUIState()
    {
        bool shouldShow = isHovered || isSelected;

        if (shouldShow && !isShowing)
        {
            isShowing = true;
            EventManager.RaiseEvent(
                new OnMoleculeCreatedEvent(moleculeData, gameObject)
            );
        }
        else if (!shouldShow && isShowing)
        {
            isShowing = false;
            EventManager.RaiseEvent(new OnMoleculeDelectedEvent());
        }
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (!MoleculeUI.Instance.canShowMoleculeInfo) return;

        isHovered = true;
        RefreshUIState();
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        isHovered = false;
        RefreshUIState();
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        isSelected = true;
        RefreshUIState();
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        isSelected = false;
        RefreshUIState();
    }
}