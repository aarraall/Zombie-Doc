using System.Collections;
using UnityEngine;

public class ParticlePoolProxy : PoolProxyBase<Component, ParticlePool<Component>>
{
    public static Component GetItem(string name, Vector3 position, Vector3 rotation = default, bool autoDismiss = false) => GetPool(name).Pull(position, rotation, autoDismiss);
    public static Component GetItem(string name, bool autoDismiss = false) => GetPool(name).Pull(autoDismiss);

    public override void Initialize()
    {
        foreach (var sample in samples)
        {
            GameObject parent = new GameObject(sample.name + "_parent");
            parent.transform.SetParent(transform);
            _pools.Add(new ParticlePool<Component>(sample, parent.transform));
        }
    }
}
