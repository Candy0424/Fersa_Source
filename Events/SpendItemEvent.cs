using PSW.Code.EventBus;
using YIS.Code.Items;

namespace YIS.Code.Events
{
    public struct SpendItemEvent : IEvent
    {
        public ItemDataSO ShopItem;

        public SpendItemEvent(ItemDataSO shopItem)
        {
            ShopItem = shopItem;
        }
    }
}