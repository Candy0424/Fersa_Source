using System.Threading.Tasks;
using CIW.Code;
using PSB.Code.BattleCode.Entities;
using PSB.Code.BattleCode.Events;
using PSW.Code.EventBus;
using UnityEngine;

namespace YIS.Code.Skills.Sequences
{
    public class HealSkillAction : ISkillAction
    {
        private Entity _target;
        private float _value;
        private HealMode _healMode;

        public HealSkillAction(Entity target, float value, HealMode healMode)
        {
            _target = target;
            _value = value;
            _healMode = healMode;
        }
        
        public async Task ExecuteAsync()
        {
            var health = _target.GetModule<EntityHealth>();
            Debug.Assert(health != null, $"{_target}에 EntityHealth가 존재하지 않습니다.");
            
            Bus<EnemyHealRequest>.Raise(new EnemyHealRequest(health, _value, _healMode));
            return;
        }
    }
}