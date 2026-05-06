using PSW.Code.EventBus;
using YIS.Code.Items;

namespace YIS.Code.Events
{
    public struct ItemVisualDataEvent : IEvent
    {
        public ItemVisualDataSO ItemVisualData;

        public ItemVisualDataEvent(ItemVisualDataSO ItemVisualData)
        {
            this.ItemVisualData = ItemVisualData;
        }
    }
}