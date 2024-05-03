using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Kalkuz.Pooling
{
  [Serializable]
  public sealed class Pool
  {
    public Queue<PoolObject> Queue { get; private set; }

    [field: SerializeField] public PoolType Type { get; set; }
    [field: SerializeField] public int InitialCapacity { get; set; }
    [field: SerializeField] public PoolObject ObjectSample { get; set; }

    private Transform _objectsParent;

    public Pool Initialize(Transform parent)
    {
      Queue = new Queue<PoolObject>();
      _objectsParent = parent;

      for (var i = 0; i < InitialCapacity; i++)
      {
        var inst = Object.Instantiate(ObjectSample, _objectsParent);

        AddToPool(inst);
        inst.OnFirstInitialize?.Invoke();
      }

      return this;
    }

    public void AddToPool(PoolObject objectRef)
    {
      Queue.Enqueue(objectRef);
    }

    public PoolObject Request(Transform parent = null)
    {
      PoolObject obj = null;

      if (Queue.Count > 0) obj = Queue.Dequeue();
      else if (Type == PoolType.Dynamic) obj = Object.Instantiate(ObjectSample, _objectsParent);

      if (!obj) return null;

      obj.InPool = false;
      obj.transform.parent = parent ? parent : _objectsParent;

      if (Type == PoolType.Static) AddToPool(obj);

      return obj;
    }
  }
}