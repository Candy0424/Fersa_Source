using System.Collections.Generic;
using PSB_Lib.Dependencies;
using PSB.Code.CoreSystem.SaveSystem;
using UnityEngine;
using YIS.Code.CoreSystem.Interfaces;
using YIS.Code.Items;

namespace YIS.Code.CoreSystem
{
    public class InventoryService : MonoBehaviour, IInventoryService, IDependencyProvider
    {
        [SerializeField] private ItemDatabase itemDatabase;
        [SerializeField] private SaveId id;
        
        [Inject] private IInventoryReader _reader;
        
        [Provide] public IInventoryService Provide() => this as IInventoryService;
        
        public Dictionary<ItemDataSO, InvenData> GetInvenData()
        {
            var inven = _reader.GetInventoryAllSlots(id.ID);

            Dictionary<ItemDataSO, InvenData> items = new Dictionary<ItemDataSO, InvenData>();

            for (int i = 0; i < inven.Length; i++)
            {
                if (inven[i].amount <= 0) continue;
                ItemDataSO item = itemDatabase.GetItemData(inven[i].itemId);
                
                if (item == null) continue;
                
                InvenData itemData = new InvenData
                {
                    slotNumber = i,
                    itemId = inven[i].itemId,
                    amount = inven[i].amount
                };

                Debug.Log($"슬롯 : {itemData.slotNumber}에 아이템 = {item.visualData.uiName}, 개수 = {itemData.amount}");
                
                items.Add(item, itemData);
            }

            return items;
        }
        
        public ItemDataSO GetItemData(int itemId) => itemDatabase.GetItemData(itemId);
    }
}