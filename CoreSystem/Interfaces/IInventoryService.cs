using System.Collections.Generic;
using PSB.Code.CoreSystem.SaveSystem;
using YIS.Code.Items;

namespace YIS.Code.CoreSystem.Interfaces
{
    public interface IInventoryService
    {
        Dictionary<ItemDataSO, InvenData> GetInvenData();
        
        ItemDataSO GetItemData(int itemId);
    }
}