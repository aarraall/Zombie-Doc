using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ParticlePool<T> : IPool<T> where T : Component
{
    T _particle;
    Transform _parent;
    private string _name;
    private List<T> _pool = new List<T>();
    private HashSet<T> _unavailables = new HashSet<T>();

    public string Name => _name;
    public List<T> Pool => _pool;
    public HashSet<T> Unavailables => _unavailables;

    public ParticlePool(T particle, Transform parent) : this(particle, particle.name, parent) { }
    public ParticlePool(T particle, string name, Transform parent)
    {
        _particle = particle;
        _name = name;
        _parent = parent;
    }
    public void Create()
    {
        T newObj = Object.Instantiate(_particle);
        newObj.gameObject.SetActive(false);
        newObj.name = _name;
        newObj.transform.SetParent(_parent);
        _pool.Add(newObj);
    }

    public T Pull()
    {
        int poolSize = _pool.Count;
        int unavailableSize = _unavailables.Count;
        T returningObj;

        if (poolSize == unavailableSize)
        {
            Create();
            returningObj = _pool.Last();
            _unavailables.Add(returningObj);
            returningObj.gameObject.SetActive(true);
            return _pool.Last();
        }

        returningObj = _pool.First(item => !_unavailables.Contains(item));
        _unavailables.Add(returningObj);
        returningObj.gameObject.SetActive(true);

        return returningObj;
    }
    public T Pull(bool autoDismiss = false)
    {

        T returningObj = Pull();
        if (autoDismiss)
        {
            ParticleSystem particle = returningObj.GetComponent<ParticleSystem>();
            PushAsync(returningObj, particle.main.duration);
        }
        return returningObj;

    }
    public T Pull(Vector3 pos, Vector3 rot = default, bool autoDismiss = false)
    {
        T returningObj = Pull();
        returningObj.transform.position = pos;
        returningObj.transform.rotation = Quaternion.Euler(rot);

        if (autoDismiss)
        {
            ParticleSystem particle = returningObj.GetComponent<ParticleSystem>();
            PushAsync(returningObj, particle.main.duration);
        }

        return returningObj;
    }
    public void Push(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(_parent);
        _unavailables.Remove(obj);
    }

    public void PushAll()
    {
        foreach (var item in _unavailables)
            Push(item);
    }
    private async void PushAsync(T obj, float delay)
    {
        CancellationTokenSource tokenSource = new CancellationTokenSource();
        try
        {
            await Task.Delay(Mathf.RoundToInt(delay * 1000), tokenSource.Token);
            if (!obj)
            {
                tokenSource?.Cancel();
            }
            else
                Push(obj);

        }
        finally
        {
            tokenSource?.Cancel();
            tokenSource?.Dispose();
        }
    }
}
