using PSB.Code.CoreSystem.SaveSystem;
using PSW.Code.EventBus;
using YIS.Code.Items;

namespace YIS.Code.Events
{
    public struct SellDataUpdateEvent : IEvent
    {
        public ItemVisualDataSO VisualData;
        public InvenData InvenItemData;

        public SellDataUpdateEvent(ItemVisualDataSO visualData, InvenData invenItemData)
        {
            VisualData = visualData;
            InvenItemData = invenItemData;
        }
    }
}