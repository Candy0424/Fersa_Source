using PSB_Lib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Serialization;

namespace YIS.Code.Effects
{
    public class PoolAnimatorEffect : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public AnimatorEffect AnimatorEffect { get; private set; }
        
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        
        public GameObject GameObject => this != null ? gameObject : null;

        private Pool _myPool;
        
        public void PlayClipEffect(Vector3 position, Quaternion rotation, int clipHash, float lifeTime = 0)
        {
            transform.SetPositionAndRotation(position, rotation);
            AnimatorEffect.PlayClip(clipHash, lifeTime);
            
        }
        
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
            AnimatorEffect.OnEndEvent += DestroyObj;
        }

        public void DestroyObj()
        {
            Debug.Log("DestroyObj");
            _myPool.Push(this);
        }

        public void ResetItem()
        {
            AnimatorEffect.GetComponent<EffectTrigger>().ResetEvent();
        }
    }
}