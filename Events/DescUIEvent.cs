using PSW.Code.EventBus;
using YIS.Code.Items;

namespace YIS.Code.Events
{
    public struct DescUIEvent : IEvent
    {
        public ItemVisualDataSO Data;

        public DescUIEvent(ItemVisualDataSO data)
        {
            Data = data;
        }
    }
}