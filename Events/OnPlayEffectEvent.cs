using System.Collections.Generic;
using CIW.Code;
using PSW.Code.EventBus;
using YIS.Code.Skills;

namespace YIS.Code.Events
{
    public struct OnPlayEffectEvent : IEvent
    {
        public BaseSkill Skill;
        public IReadOnlyList<Entity> Targets;

        public OnPlayEffectEvent(BaseSkill skill, IReadOnlyList<Entity> targets)
        {
            Skill = skill;
            Targets = targets;
        }
    }
}