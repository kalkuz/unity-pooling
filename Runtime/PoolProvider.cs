using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kalkuz.Pooling
{
  public sealed class PoolProvider : MonoBehaviour
  {
    private static PoolProvider m_instance;

    private static readonly Dictionary<string, Pool> PoolDictionary = new();

    [SerializeField] private List<Pool> pools;

    private void Awake()
    {
      if (!m_instance) m_instance = this;
      else Destroy(this);

      foreach (var pool in pools)
      {
        AssignNewPool(pool);
      }
    }

    public static void AssignNewPool(Pool newPool)
    {
      if (!m_instance)
      {
        throw new Exception("No PoolProvider instance is initialized! Failed to use object pooling");
      }

      var go = new GameObject($"{newPool.ObjectSample.PoolId} Pool")
      {
        transform = { parent = m_instance.transform }
      };
      newPool.Initialize(go.transform);

      PoolDictionary[newPool.ObjectSample.PoolId] = newPool;
    }

    public static Pool GetPool(PoolObject po)
    {
      if (!m_instance)
      {
        throw new Exception("No PoolProvider instance is initialized! Failed to use object pooling");
      }
      
      var id = po.PoolId;

      if (PoolDictionary.TryGetValue(id, out var value))
      {
        return value;
      }

      PoolDictionary[id] = new Pool()
      {
        ObjectSample = po,
        Type = PoolType.Dynamic,
      };
      
      AssignNewPool(PoolDictionary[id]);
      return PoolDictionary[id];
    }
  }
}