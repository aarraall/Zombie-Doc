using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PoolProxyBase<TSample, TPool> : MonoBehaviour where TPool : IPool<TSample> where TSample : Object

{
    [SerializeField] protected TSample[] samples;

    protected static List<TPool> _pools = new List<TPool>();
    public static TPool GetPool(string name) => _pools.First(pool => pool.Name == name);
    public static void Dismiss(TSample item) => GetPool(item.name).Push(item);
    public static void DismissAll() => _pools.ForEach(pool => pool.PushAll());
    public abstract void Initialize();
 }
