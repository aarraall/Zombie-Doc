using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolProxy : PoolProxyBase<MonoBehaviour, GameObjectPool<MonoBehaviour>>
{
    public static MonoBehaviour GetItem(string name) => GetPool(name).Pull();
    public static MonoBehaviour GetItem(string name, Vector3 position, Vector3 rotation = default) => GetPool(name).Pull(position, rotation);

    private void Awake()
    {
        Initialize();
    }
    public override void Initialize()
    {
        foreach (var sample in samples)
        {
            GameObject parent = new GameObject(sample.name + "_parent");
            parent.transform.SetParent(transform);
            _pools.Add(new GameObjectPool<MonoBehaviour>(sample, parent.transform));
        }
    }
}
