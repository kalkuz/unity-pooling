using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KalkuzSystems.Pooling
{
  [Serializable]
  public sealed class Pool
  {
    private Queue<PoolObject> queue;

    [SerializeField] internal PoolType type;
    [SerializeField] private int initialCapacity;
    [SerializeField] internal PoolObject objectSample;

    public Queue<PoolObject> Queue => queue;
    public PoolType Type => type;
    public int InitialCapacity => initialCapacity;
    public PoolObject ObjectSample => objectSample;

    private Transform objectsParent;

    public Pool Initialize(Transform parent)
    {
      queue = new Queue<PoolObject>();
      objectsParent = parent;

      for (var i = 0; i < initialCapacity; i++)
      {
        var inst = Object.Instantiate(objectSample, objectsParent);

        AddToPool(inst);
        inst.OnFirstInitialize?.Invoke();
      }

      return this;
    }

    public void AddToPool(PoolObject objectRef)
    {
      queue.Enqueue(objectRef);
    }

    public PoolObject Request()
    {
      PoolObject obj = null;
      if (queue.Count > 0) obj = queue.Dequeue();
      else if (type == PoolType.DYNAMIC) obj = Object.Instantiate(objectSample, objectsParent);

      if (!obj) return null;

      obj.InPool = false;
      obj.transform.parent = objectsParent;
      if (type == PoolType.STATIC) AddToPool(obj);
      
      return obj;
    }
  }
}