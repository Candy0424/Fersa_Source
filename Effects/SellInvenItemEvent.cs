using PSW.Code.EventBus;

namespace YIS.Code.Effects
{
    public struct SellInvenItemEvent : IEvent
    {
        public int SlotNumber;
        public int Amount;

        public SellInvenItemEvent(int slotNumber, int amount)
        {
            SlotNumber = slotNumber;
            Amount = amount;
        }
    }
}