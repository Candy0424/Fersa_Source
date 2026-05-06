using System.Collections.Generic;
using System.Threading.Tasks;
using CIW.Code;
using PSW.Code.EventBus;
using UnityEngine;
using YIS.Code.Events;

namespace YIS.Code.Skills.Sequences
{
    public class PlayEffectCallbackAction : ISkillAction
    {
        private readonly BaseSkill _skill;
        private readonly IReadOnlyList<Entity> _targets;

        public PlayEffectCallbackAction(BaseSkill skill, IReadOnlyList<Entity> targets)
        {
            _skill = skill;
            _targets = targets;
        }
        
        public async Task ExecuteAsync()
        {
            if (_skill == null || _targets == null) return;
            
            Bus<OnPlayEffectEvent>.Raise(new OnPlayEffectEvent(_skill, _targets));
        }
    }
}