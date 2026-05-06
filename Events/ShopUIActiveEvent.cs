using PSW.Code.EventBus;

namespace YIS.Code.Events
{
    public struct ShopUIActiveEvent : IEvent
    {
        public bool IsActive;

        public ShopUIActiveEvent(bool isActive)
        {
            this.IsActive = isActive;
        }

    }
}