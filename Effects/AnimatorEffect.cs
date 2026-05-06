using System;
using PSB_Lib.ObjectPool.RunTime;
using UnityEngine;

namespace YIS.Code.Effects
{
    public class AnimatorEffect : MonoBehaviour
    {
        [SerializeField] private bool onlyEndDestroy = true;
        [SerializeField] private Animator _animator;

        public PoolItemSO PoolItem { get; }
        public GameObject GameObject { get; }
        
        private float _startTime;
        private float _endTime;

        public event Action OnEndEvent;

        public void PlayClip(int clipHash, float lifeTime = 0)
        {
            _animator.Play(clipHash, -1, 0);
            _startTime = Time.time;
            _endTime = lifeTime;
            
            onlyEndDestroy = lifeTime <= 0;
        }

        private void Update()
        {
            if (onlyEndDestroy) return;
            
            float passedTime = Time.time - _startTime;
            if (passedTime > _endTime)
            {
                OnEndEvent?.Invoke();
            }
        }
    }
}