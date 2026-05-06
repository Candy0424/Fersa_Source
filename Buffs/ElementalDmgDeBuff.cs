using PSB.Code.BattleCode.Entities;
using YIS.Code.Combat;

namespace YIS.Code.Modules
{
    public class ElementalDmgDeBuff : Buff
    {
        private float _value;
        protected override void ApplyBuff(float value, int duration)
        {
            _value = value;
            PlayEffect();
        }

        public override void UpdateBuffTime()
        {
            base.UpdateBuffTime();
            var health = owner.GetModule<EntityHealth>();
            DamageData data = new DamageData
            {
                Damage = _value,
                ElementalType = BuffDataSO.elemental
            };
            health.ApplyDamage(data);
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
            StopEffect();
        }
    }
}