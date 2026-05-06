using PSW.Code.EventBus;
using UnityEngine;

namespace YIS.Code.Events
{
    public struct DescUIActiveEvent : IEvent
    {
        public bool IsActive;

        public DescUIActiveEvent(bool isActive)
        {
            IsActive = isActive;
        }
    }
}