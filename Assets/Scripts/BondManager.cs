using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BondManager : MonoBehaviour
{
    public List<MoleculeData> molecules;

    private List<AtomController> currentAtoms = new List<AtomController>();

    [SerializeField] MoleculeDatabase moleculeDatabase;

    public AtomPool atomPool;   
    [Header("Animation Settings")]
    public float animationDuration = 0.6f;
    public float zigzagStrength = 0.02f;
    public float rotationSpeed = 120f;
    public int zigzagFrequency = 4;

    private void OnEnable()
    {
        EventManager.ListenEvent<OnMoleculeInfoRequestedEvent>(OnMoleculeInfoRequested);
    }

    private void OnMoleculeInfoRequested(OnMoleculeInfoRequestedEvent e)
    {
         e.moleculeData?.Invoke(moleculeDatabase.GetMoleculeByType(e.moleculeType));
    }

    public void AddAtom(AtomController atom)
    {
        if (!currentAtoms.Contains(atom))
        {
            currentAtoms.Add(atom);
            Debug.Log(atom.atomType + " added");

            CheckMolecule();
        }
    }

    public void RemoveAtom(AtomController atom)
    {
        if (currentAtoms.Contains(atom))
        {
            currentAtoms.Remove(atom);
            Debug.Log(atom.atomType + " removed");
        }
    }

    void CheckMolecule()
    {
        foreach (var molecule in moleculeDatabase.GetAllMolecules())
        {
            bool valid = true;

            foreach (var req in molecule.requirements)
            {
                int count = currentAtoms.Count(a => a.atomType == req.atomType);

                if (count != req.count)
                {
                    valid = false;
                    break;
                }
            }

            if (valid && currentAtoms.Count == molecule.requirements.Sum(r => r.count))
            {
                Debug.Log("MATCH: " + molecule.moleculeName);
                SpawnMolecule(molecule);
                return;
            }
        }
    }

    void SpawnMolecule(MoleculeData molecule)
    {
        Vector3 spawnPos = transform.position;

        GameObject mol = Instantiate(molecule.moleculePrefab, spawnPos, Quaternion.identity);

        StartCoroutine(AnimateMolecule(mol));

        foreach (var atom in currentAtoms)
            atomPool.ReturnToPool(atom);

        currentAtoms.Clear();
    }

    IEnumerator AnimateMolecule(GameObject molecule)
    {
        foreach (Transform child in molecule.transform)
        {
            StartCoroutine(AnimateChild(child));
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator AnimateChild(Transform child)
    {
        Vector3 originalPos = child.localPosition;
        Quaternion originalRot = child.localRotation;

        float t = 0;

        // Animate out (zig-zag + rotate)
        while (t < animationDuration)
        {
            t += Time.deltaTime;
            float progress = t / animationDuration;

            float zigzag = Mathf.Sin(progress * Mathf.PI * zigzagFrequency) * zigzagStrength;

            child.localPosition = originalPos + new Vector3(zigzag, 0, 0);
            child.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

            yield return null;
        }

        t = 0;

        // Return back
        while (t < animationDuration)
        {
            t += Time.deltaTime;
            float progress = t / animationDuration;

            child.localPosition = Vector3.Lerp(child.localPosition, originalPos, progress);
            child.localRotation = Quaternion.Slerp(child.localRotation, originalRot, progress);

            yield return null;
        }

        child.localPosition = originalPos;
        child.localRotation = originalRot;
    }

    private void OnDisable()
    {
        EventManager.StopListening<OnMoleculeInfoRequestedEvent>(OnMoleculeInfoRequested);
    }

}