using System;
using CIW.Code;
using PSB_Lib.ObjectPool.RunTime;
using PSB.Code.BattleCode.Players;
using PSW.Code.EventBus;
using UnityEngine;
using Work.CSH.Scripts.Interfaces;
using YIS.Code.Defines;
using YIS.Code.Modules;

namespace YIS.Code.Modules
{
    public abstract class Buff : MonoBehaviour, IPoolable
    {
        [field : SerializeField] public BuffDataSO BuffDataSO { get; protected set; }
        [field: SerializeField] public ParticleSystem BuffEffect { get; protected set; }

        private int _baseTurn;
        private float _value ;
        
        protected int currentTurn;
        protected bool isActive;
        protected bool appliedThisTurn;

        protected Pool myPool;
        protected Entity owner;
        
        public int RemainingTurn => currentTurn;
        public float Value => _value;
        public bool IsActive => isActive;

        public virtual void BuffInit(ModuleOwner entity,float value, int duration, Transform parentRoot)
        {
            ResetItem();
            owner = entity as Entity;
            _baseTurn = duration;
            _value = value;
            currentTurn = 0;
            StopEffect();
            transform.SetParent(parentRoot);
            transform.localPosition = Vector3.zero;
            BuffApply(value, duration);
            AfterInit();
        }

        protected virtual void AfterInit()
        {
            
        }

        #region PoolingSection

        public PoolItemSO PoolItem => BuffDataSO.buffPrefab;
        public GameObject GameObject => gameObject;
        
        public void SetUpPool(Pool pool)
        {
            myPool = pool;
        }

        public void ResetItem()
        {
            if (isActive)
                RemoveBuff();
            
            isActive = false;
        }

        #endregion
        
        private void SetBuffInfo(float value, int duration = 0)
        {
            currentTurn = duration;
            isActive = true;
        }
        
        protected virtual void BuffApply(float value, int duration = 0)
        {
            SetBuffInfo(value, duration);
            appliedThisTurn = true;
            ApplyBuff(value, duration);
        }

        public virtual void UpdateBuffTime()
        {
            if (!isActive) return;
            
            if (appliedThisTurn)
            {
                appliedThisTurn = false;
                return;
            }

            currentTurn--;
            if (currentTurn <= 0)
                OnBuffRemove();
        }

        public virtual void OnBuffRemove()
        {
            if (!isActive) return;

            isActive = false;
            currentTurn = 0;

            StopEffect();
            RemoveBuff();
            myPool.Push(this);
        }

        protected abstract void ApplyBuff(float value, int duration);

        protected virtual void PlayEffect()
        {
            BuffEffect.gameObject.SetActive(true);
            BuffEffect.Play();
        }

        protected virtual void StopEffect()
        {
            BuffEffect.Stop();
            BuffEffect.gameObject.SetActive(false);
        }

        protected abstract void RemoveBuff();
    }
}