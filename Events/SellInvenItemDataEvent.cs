using PSB.Code.CoreSystem.SaveSystem;
using PSW.Code.EventBus;

namespace YIS.Code.Events
{
    public struct SellInvenItemDataEvent : IEvent
    {
        public InvenData InvenItemData;

        public SellInvenItemDataEvent(InvenData invenItemData)
        {
            InvenItemData = invenItemData;
        }
    }
}