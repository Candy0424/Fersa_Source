using PSW.Code.EventBus;
using YIS.Code.Items;

namespace YIS.Code.Events
{
    public struct SellItemDataEvent : IEvent
    {
        public ItemSellDataSO ItemSellData;
        public int Amount;
        public int SlotNumber;

        public SellItemDataEvent(ItemSellDataSO itemSellData, int amount, int slotNumber)
        {
            ItemSellData = itemSellData;
            Amount = amount;
            SlotNumber = slotNumber;
        }
    }
}