﻿using UnityEngine;
using UnityEngine.Events;

namespace Kalkuz.Pooling
{
  public class PoolObject : MonoBehaviour
  {
    [SerializeField] protected string id;
    [SerializeField] protected UnityEvent onFirstInitialize;
    private bool inPool;

    public string ID => id;
    public UnityEvent OnFirstInitialize => onFirstInitialize;

    public bool InPool
    {
      get => inPool;
      set => inPool = value;
    }

    public void ReturnToPool()
    {
      if (inPool) return;
      
      var pool = PoolProvider.GetPool(this);
      pool.AddToPool(this);
      inPool = true;
    }
  }
}