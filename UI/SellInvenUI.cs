using System.Collections.Generic;
using PSB.Code.CoreSystem.SaveSystem;
using PSW.Code.EventBus;
using UnityEngine;
using UnityEngine.UI;
using YIS.Code.Events;
using YIS.Code.Items;

namespace YIS.Code.Combat
{
    public class SellInvenUI : MonoBehaviour
    {
        [SerializeField] private Transform itemsRoot;
        [SerializeField] private GameObject sellItemPrefab;
        [SerializeField] private Button closeBtn;

        private Dictionary<ItemVisualDataSO, SellItemPanelUI> _items;

        private SellItemPanelUI _curSellItemPanel;

        private void Awake()
        {
            _items = new Dictionary<ItemVisualDataSO, SellItemPanelUI>();
        }

        private void OnEnable()
        {
            closeBtn.onClick.AddListener(HandleCloseBtnClick);
        }

        private void OnDisable()
        {
            closeBtn.onClick.RemoveAllListeners();
        }

        private void HandleCloseBtnClick()
        {
            Bus<SellShopUIActiveEvent>.Raise(new SellShopUIActiveEvent(false));
        }

        public void ResetData()
        {
            if (_items.Count == 0) return;

            foreach (var item in _items)
            {
                item.Value.ResetData();
            }
        }

        public void AddItem(ItemVisualDataSO item, InvenData invenItemData, int gold)
        {
            if (!_items.ContainsKey(item))
            {
                GameObject obj = Instantiate(sellItemPrefab, itemsRoot);
                SellItemPanelUI sellItem = obj.GetComponent<SellItemPanelUI>();
                _items.Add(item, sellItem);
            }

            if (_items[item].IsDisable)
            {
                _items[item].Initialize();
                _items[item].EnableData();
                _items[item].AddEvent(() =>
                {
                    ChangeFocus(_items[item]);
                    _curSellItemPanel = _items[item];
                    Bus<SellDataUpdateEvent>.Raise(new SellDataUpdateEvent(item, invenItemData));
                });
            }

            _items[item].SetData(item, invenItemData.amount, gold);
        }

        private void ChangeFocus(SellItemPanelUI newItem)
        {
            if (_curSellItemPanel == null)
            {
                _curSellItemPanel = newItem;
                _curSellItemPanel.SetActiveItem(true, false);
                return;
            }

            _curSellItemPanel.SetActiveItem(false, false);
            _curSellItemPanel = newItem;
            _curSellItemPanel.SetActiveItem(true, false);
            
        }
    }
}