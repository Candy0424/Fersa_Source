using PSW.Code.EventBus;
using YIS.Code.Skills;

namespace YIS.Code.Events
{
    public struct SpendSkillEvent : IEvent
    {
        public SkillDataSO ShopSkill;

        public SpendSkillEvent(SkillDataSO shopSkill)
        {
            ShopSkill = shopSkill;
        }
    }
}