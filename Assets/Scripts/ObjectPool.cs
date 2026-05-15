using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private Transform _container;
    [SerializeField] private int _prewarmCount = 10;

    private Queue<T> _pool = new Queue<T>();
    private List<T> _activeObjects = new List<T>();

    private void Awake()
    {
        for (int i = 0; i < _prewarmCount; i++)
        {
            CreateNewObject();
        }
    }

    private T CreateNewObject()
    {
        T obj = Instantiate(_prefab, _container);
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
        return obj;
    }

    public T GetObject()
    {
        T obj = _pool.Count > 0 ? _pool.Dequeue() : CreateNewObject();
        _activeObjects.Add(obj);
        return obj;
    }

    public void PutObject(T obj)
    {
        obj.gameObject.SetActive(false);
        _activeObjects.Remove(obj);
        _pool.Enqueue(obj);
    }

    public void Reset()
    {
        for (int i = _activeObjects.Count - 1; i >= 0; i--)
        {
            T obj = _activeObjects[i];
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
        _activeObjects.Clear();
    }
}