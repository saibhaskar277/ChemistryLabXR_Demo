using System.Collections.Generic;
using UnityEngine;

public class AtomPool : MonoBehaviour
{
    private readonly Dictionary<AtomType, Queue<GameObject>> pool = new();

    public GameObject Spawn(AtomType type, Vector3 position, GameObject prefab)
    {
        if (!pool.ContainsKey(type))
            pool[type] = new Queue<GameObject>();

        GameObject obj;

        if (pool[type].Count > 0)
        {
            obj = pool[type].Dequeue();
        }
        else
        {
            obj = Instantiate(prefab);
        }

        obj.transform.position = position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);

        return obj;
    }

    public void ReturnToPool(AtomController atom)
    {
        var type = atom.atomType;

        if (!pool.ContainsKey(type))
            pool[type] = new Queue<GameObject>();

        atom.gameObject.SetActive(false);
        pool[type].Enqueue(atom.gameObject);
    }
}
