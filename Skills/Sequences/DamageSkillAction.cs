using System.Collections.Generic;
using System.Threading.Tasks;
using CIW.Code;
using Code.Scripts.Entities;
using PSB.Code.BattleCode.Entities;
using PSW.Code.EventBus;
using UnityEngine;
using YIS.Code.Combat;
using YIS.Code.Defines;
using YIS.Code.Events;
using YIS.Code.Modules;

namespace YIS.Code.Skills.Sequences
{
    public class DamageSkillAction : ISkillAction
    {
        private readonly Entity _user;
        private readonly float _impulseValue;
        private IReadOnlyList<Entity> _targets;
        private DamageData _damageData;
        
        public IReadOnlyList<Entity> Targets => _targets;
        public DamageData CurrentDamageData => _damageData;

        public DamageSkillAction(Entity user, IReadOnlyList<Entity> targets, DamageData damageData, float impulseValue)
        {
            _user = user;
            _targets = targets;
            _damageData = damageData;
            _impulseValue = impulseValue;
        }
        
        public async Task ExecuteAsync()
        {
            if (_targets.Count <= 0 || _user == null) return;
            
            Elemental finalElemental = CalculateElementalSkill(_damageData.ElementalType, _user);
            EntityDamageCalcModule dmgCalc = _user.GetModule<EntityDamageCalcModule>();
            
            Debug.Assert(dmgCalc != null, $"해당 엔티티에 데미지 계산 모듈이 없습니다. {_user.name}");
            
            _damageData.ElementalType = finalElemental;
            float baseDamage = _damageData.Damage;
            foreach (var target in _targets)
            {
                EntityHealth health = target.GetModule<EntityHealth>();
                EntityStat stat = target.GetModule<EntityStat>();
                
                Debug.Log($"데미지 속성: {_damageData.ElementalType}");
                
                int damage = dmgCalc.DamageCalc(_damageData, stat);
                _damageData.Damage = damage;
                
                health.ApplyDamage(_damageData);
                Bus<DamageActionAfterEvent>.Raise(new DamageActionAfterEvent(_damageData, target));
                _damageData.Damage = baseDamage;
            }
            
            Vector3 hitDirection = Vector3.zero;
            
            if (_user.transform != null && _targets[0].transform != null)
            {
                Vector3 direction = _targets[0].transform.position - _user.transform.position;
                
                direction.y = 0f; 
                direction.z = 0f; 

                hitDirection = direction.normalized;
            }

            Vector3 finalImpulseVelocity = hitDirection * _impulseValue;

            Bus<ImpulseEvent>.Raise(new ImpulseEvent(finalImpulseVelocity));
        }
        
        public Elemental CalculateElementalSkill(Elemental baseElemental, Entity user)
        {
            Elemental result = baseElemental;

            BuffModule buffModule = user.GetModule<BuffModule>(); 
            if (buffModule != null && buffModule.TryGetElementalOverrideOrImmediately(out Elemental overrideElemental))
                result = overrideElemental;

            return result;
        }
    }
}