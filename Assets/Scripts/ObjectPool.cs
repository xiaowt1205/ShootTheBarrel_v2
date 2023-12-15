using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private Queue<T> _objectQuene;
    private GameObject _prefab;

    private static ObjectPool<T> _instance = null;
    public static ObjectPool<T> instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ObjectPool<T>();
            }
            return _instance;
        }
    }

    public int queneCount
    {
        get
        {
            return _objectQuene.Count;
        }
    }

    public void InitPool(GameObject _go)
    {
        _prefab = _go;
        _objectQuene = new Queue<T>();
    }

    public T Spawn(Transform _parents)
    {
        if (_prefab == null)
        {
            return default(T);
        }

        if (queneCount <= 0)
        {
            GameObject _go = Object.Instantiate(_prefab);
            T _t = _go.GetComponent<T>();

            if (_t == null)
            {
                return default(T);
            }

            _objectQuene.Enqueue(_t);
        }
        T _obj = _objectQuene.Dequeue();
        _obj.gameObject.transform.position = _parents.position;
        _obj.gameObject.transform.rotation = _parents.rotation;
        _obj.gameObject.SetActive(true);
        return _obj;
    }

    public void Recycle(T _obj)
    {
        _objectQuene.Enqueue(_obj);
        _obj.gameObject.SetActive(false);
    }
}