using System;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Pooling
{
  public sealed class PoolProvider : MonoBehaviour
  {
    private static PoolProvider m_instance;

    private readonly Dictionary<string, Pool> poolDictionary = new();

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

      var go = new GameObject($"{newPool.ObjectSample.ID} Pool")
      {
        transform = { parent = m_instance.transform }
      };
      newPool.Initialize(go.transform);

      m_instance.poolDictionary[newPool.ObjectSample.ID] = newPool;
    }

    public static Pool GetPool(string id)
    {
      if (!m_instance)
      {
        throw new Exception("No PoolProvider instance is initialized! Failed to use object pooling");
      }

      if (m_instance.poolDictionary.TryGetValue(id, out var value)) return value;

      throw new Exception("There is no pool found for object id: " + id);
    }
  }
}