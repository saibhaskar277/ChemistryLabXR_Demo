using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSocketInteractor))]
public class MoleculeBuildValidator : MonoBehaviour
{
    [SerializeField] private MoleculeData targetMolecule;

    private XRSocketInteractor socket;

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    private void OnEnable()
    {
        socket.selectEntered.AddListener(OnAtomInserted);
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnAtomInserted);
    }

    private void OnAtomInserted(SelectEnterEventArgs args)
    {
        var atom = args.interactableObject.transform.GetComponent<AtomController>();
        if (atom == null) return;

        if (!ValidateAtom(atom.atomType))
        {
            Debug.Log($"{atom.atomType} is not part of {targetMolecule.moleculeType}");

            socket.interactionManager.SelectExit(
                socket,
                args.interactableObject
            );
        }
        else
        {
            Destroy(atom.gameObject);
            EventManager.RaiseEvent(new OnAtomAddedEvent(atom.atomType));
        }
    }

    public bool ValidateAtom(AtomType atom)
    {
        return targetMolecule.requirements.Exists(
            req => req.atomType == atom
        );
    }
}