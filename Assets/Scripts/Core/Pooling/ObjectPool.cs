using UnityEngine;
using System.Collections.Generic;

public class ObjectPool<T> where T : Component, IPool {

    private readonly T prefab;
    private readonly Transform parent;
    private readonly Queue<T> poolQueue;

    public ObjectPool(T prefab, int initialSize, Transform parent) {

        this.prefab = prefab;
        this.parent = parent;
        poolQueue = new Queue<T>();

        Generate(initialSize);
    }

    private void Generate(int size) {

        for(int i = 0; i < size; i++) {

            T obj = CreateInstance();
            obj.gameObject.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }

    private T CreateInstance() {

        T instance = Object.Instantiate(prefab, parent);
        return instance;
    }

    public T Get() {

        T obj = poolQueue.Count > 0 ? poolQueue.Dequeue() : CreateInstance();

        obj.gameObject.SetActive(true);
        obj.OnSpawned();
        return obj;
    }

    public void ReturnToPool(T obj) {

        obj.OnDespawned();
        obj.gameObject.SetActive(false);
        poolQueue.Enqueue(obj);
    }
}