using PSW.Code.EventBus;

namespace YIS.Code.Events
{
    public struct SellShopUIActiveEvent : IEvent
    {
        public bool IsActive;

        public SellShopUIActiveEvent(bool isActive)
        {
            IsActive = isActive;
        }
    }
}