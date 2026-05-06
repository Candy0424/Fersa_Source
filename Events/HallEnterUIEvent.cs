using PSW.Code.EventBus;

namespace YIS.Code.Events
{
    public struct HallEnterUIEvent : IEvent
    {
        public bool IsActive;

        public HallEnterUIEvent(bool isActive)
        {
            IsActive = isActive;
        }
    }
}