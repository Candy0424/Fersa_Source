using PSB_Lib.Dependencies;
using PSW.Code.EventBus;
using UnityEngine;
using YIS.Code.CoreSystem.Interfaces;
using YIS.Code.Events;

namespace YIS.Code.Combat
{
    // 얘 버그 있어요 shopCanvas 알파값만 줄이지 말고 얘도 꺼줘
    public class ItemSellUI : MonoBehaviour
    {
        [SerializeField] private SellInvenUI invenUI;

        [Inject] private IInventoryService _invenService;

        private void Awake()
        {
            Bus<SellShopUIActiveEvent>.OnEvent += SetActive;
            
            gameObject.SetActive(false);
        }
        
        private void OnDestroy()
        {
            Bus<SellShopUIActiveEvent>.OnEvent -= SetActive;
            Bus<SellInvenItemDataEvent>.OnEvent -= HandleSellInvenItem;
        }

        private void SetActive(SellShopUIActiveEvent evt)
        {
            if (evt.IsActive)
            {
                gameObject.SetActive(true);
                Bus<SellInvenItemDataEvent>.OnEvent -= HandleSellInvenItem;
                Bus<SellInvenItemDataEvent>.OnEvent += HandleSellInvenItem;
                InventoryLoad();
            }
            else
            {
                Bus<SellInvenItemDataEvent>.OnEvent -= HandleSellInvenItem;
                gameObject.SetActive(false);
            }
        }

        private void HandleSellInvenItem(SellInvenItemDataEvent evt)
        {
            var item = _invenService.GetItemData(evt.InvenItemData.itemId);
            Bus<SellItemDataEvent>.Raise(new SellItemDataEvent(item.sellData, evt.InvenItemData.amount, evt.InvenItemData.slotNumber));
            InventoryLoad();
        }

        private void InventoryLoad()
        { 
            var inven = _invenService.GetInvenData();

            invenUI.ResetData();
            
            foreach (var item in inven)
            {
                invenUI.AddItem(item.Key.visualData, item.Value, item.Key.sellData.sellValue);
            }
        }
        
    }
}