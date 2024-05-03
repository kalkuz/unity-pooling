using Kalkuz.Utility.TagSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Kalkuz.Pooling
{
  public class PoolObject : MonoBehaviour
  {
    [field: SerializeField, Header("Pooling"), TagSelect("PoolId")] public string PoolId { get; protected set; }
    [field: SerializeField] public UnityEvent OnFirstInitialize { get; protected set; }
    [field: SerializeField] public UnityEvent OnReturnToPool { get; protected set; }
    
    public bool InPool { get; set; }

    public virtual void ReturnToPool()
    {
      if (InPool) return;
      
      var pool = PoolProvider.GetPool(this);
      pool.AddToPool(this);
      InPool = true;
      
      OnReturnToPool?.Invoke();
    }
  }
}