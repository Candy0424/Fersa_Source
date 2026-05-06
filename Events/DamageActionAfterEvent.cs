using System.Collections.Generic;
using CIW.Code;
using PSW.Code.EventBus;
using YIS.Code.Combat;

namespace YIS.Code.Events
{
    public struct DamageActionAfterEvent : IEvent
    {
        public DamageData DamageData;
        public Entity Target;

        public DamageActionAfterEvent(DamageData damageData, Entity targets)
        {
            this.DamageData = damageData;
            this.Target = targets;
        }
    }
}