using System;
using Code.Scripts.Entities;
using PSB_Lib.StatSystem;
using UnityEngine;
using YIS.Code.Defines;

namespace YIS.Code.Modules
{
    public class DamageUpBuff : Buff
    {
        [SerializeField] private StatSO attackDamageStat;

        protected override void ApplyBuff(float value, int duration)
        {
            var statModule = owner.GetModule<EntityStat>();
            statModule.AddModifier(attackDamageStat, this, value);
            PlayEffect();
            Debug.Log($"Buff : {value} | stat : {attackDamageStat.Value}");
        }

        public override void UpdateBuffTime()
        {
            base.UpdateBuffTime();
        }

        protected override void PlayEffect()
        {
            base.PlayEffect();
        }

        protected override void StopEffect()
        {
            base.StopEffect();
        }

        protected override void RemoveBuff()
        {
            var statModule = owner.GetModule<EntityStat>();
            statModule.RemoveModifier(attackDamageStat, this);
            StopEffect();
        }
    }
}