using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenericPool<T> : IPool<T>
{
    protected string _name;   
    protected List<T> pool = new List<T>();
    protected HashSet<T> _unavailables = new HashSet<T>();
    protected Func<T> OnCreate;
    protected Action<T> OnDismiss, OnGet;

    public string Name => _name;
    public List<T> Pool => pool;
    public HashSet<T> Unavailables => _unavailables;

    public GenericPool(Func<T> onCreate, Action<T> onDismiss = null, Action<T> onGet = null)
    {
        OnCreate = onCreate;
        OnDismiss = onDismiss;
        OnGet = onGet;
    }

    public T Pull()
    {
        int poolSize = Pool.Count;
        int unavailableSize = Unavailables.Count;
        T returningObj;

        if (poolSize == unavailableSize)
        {
            Create();
            returningObj = Pool.Last();
            Unavailables.Add(returningObj);
            return Pool.Last();
        }

        returningObj = Pool.First(item => !Unavailables.Contains(item));
        Unavailables.Add(returningObj);

        OnGet?.Invoke(returningObj);
        return returningObj;
    }

    public void Create()
    {
        T tempObj = OnCreate.Invoke();
        Pool.Add(tempObj);
    }

    public void Push(T obj)
    {
        Unavailables.Remove(obj);
        OnDismiss.Invoke(obj);
    }
    public void PushAll()
    {
        foreach (var item in Unavailables)
            Push(item);
    }
}



public interface IPool<T>
{
    public string Name { get; }
    public List<T> Pool { get;}
    public HashSet<T> Unavailables { get;}
    public T Pull();
    public void Create();
    public void Push(T obj);
    public void PushAll();
}

