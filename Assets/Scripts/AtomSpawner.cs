using UnityEngine;
using System.Collections.Generic;

public class AtomSpawner : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private AtomDatabase database;

    [Header("UI")]
    [SerializeField] private Transform contentRoot;
    [SerializeField] private AtomViewItem itemPrefab;

    [Header("Spawn")]
    [SerializeField] private AtomPool atomPool;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        BuildUI();
    }

    private void BuildUI() 
    {
        foreach (var atom in database.atoms)
        {
            var item = Instantiate(itemPrefab, contentRoot);
            item.Bind(atom, SpawnAtom);
        }
    }

    private void SpawnAtom(AtomType type)
    {
        atomPool.Spawn(type, spawnPoint.position, database.GetAtomDataByType(type).prefab);
    }
}