using PSW.Code.EventBus;

namespace YIS.Code.Events
{
    public struct StartStatSettingUIActiveEvent : IEvent
    {
        public bool IsActive;

        public StartStatSettingUIActiveEvent(bool isActive)
        {
            this.IsActive = isActive;
        }
    }
}