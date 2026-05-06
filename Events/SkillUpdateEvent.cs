using System.Collections.Generic;
using PSW.Code.EventBus;
using YIS.Code.Skills;

namespace YIS.Code.Events
{
    public struct SkillUpdateEvent : IEvent
    {
        public List<SkillDataSO> Skills;

        public SkillUpdateEvent(List<SkillDataSO> skills)
        {
            Skills = skills;
        }
    }
}