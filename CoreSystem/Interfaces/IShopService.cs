using System.Collections.Generic;
using YIS.Code.Items;

namespace YIS.Code.CoreSystem.Interfaces
{
    public interface IShopService
    {
        void InitItems();
        List<ShopItemDataSO> LoadItems();
        List<ShopItemDataSO> LoadSkills();
        List<ShopItemDataSO> LoadHpItems();
        List<DetailDataSO> LoadDetailData();
        bool SpendItem(ShopItemDataSO item);
        bool RestoreItem(int price);
        bool SellItem(ItemSellDataSO item, int amount, int slotNumber);
    }
}