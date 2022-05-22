using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectPool<T> : IPool<T> where T : MonoBehaviour
{
    T _prefab;
    Transform _parent;
    private string _name;
    private List<T> _pool = new List<T>();
    private HashSet<T> _unavailables = new HashSet<T>();
    public List<T> Pool => _pool;
    public HashSet<T> Unavailables => _unavailables;
    public string Name => _name;

    public GameObjectPool(T prefab, Transform parent) : this(prefab, prefab.name, parent) { }
    public GameObjectPool(T prefab, string name, Transform parent)
    {
        _prefab = prefab;
        _name = name;
        _parent = parent;
    }

    public void Create()
    {
        T newObj = Object.Instantiate(_prefab);
        newObj.gameObject.SetActive(false);
        newObj.name = _name;
        newObj.transform.SetParent(_parent);
        _pool.Add(newObj);
    }

    public void Push(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(_parent);
        _unavailables.Remove(obj);
    }

    public void PushAll()
    {
        foreach (var obj in _unavailables)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_parent);
        }

        _unavailables.Clear();
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
    public T Pull(Vector3 pos, Vector3 rot = default)
    {
        T returningObj = Pull();
        returningObj.transform.position = pos;
        returningObj.transform.rotation = Quaternion.Euler(rot);
        return returningObj;
    }
}
