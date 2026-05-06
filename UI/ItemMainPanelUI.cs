using CSH.Scripts.Items;
using CSH.Scripts.UIs;
using PSB.Code.BattleCode.UIs;
using PSB.Code.CoreSystem.Events;
using PSB.Code.CoreSystem.SaveSystem;
using PSW.Code.EventBus;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Work.CSH.Scripts.Enums;
using Work.CSH.Scripts.UIs;
using YIS.Code.Events;
using YIS.Code.Items;

namespace YIS.Code.Combat
{
    public class ItemMainPanelUI : MonoBehaviour, IScrollHandler
    {
        [Header("References")]
        [SerializeField] private ItemDeleteUI deletePopup;
        [SerializeField] private InventoryCode inventory;
        [SerializeField] private Transform itemParentTrm;
        [SerializeField] private QuickSlotRegister quickSlot;

        [Header("Prefab")]
        [SerializeField] private ItemPanelUI itemPanelPrefab;
        
        [Header("Animation")]
        private bool _isOpened;

        private ScrollRect _scrollRect;
        private readonly List<ItemPanelUI> _itemPanels = new();

        private ItemPanelUI _currentItem;

        private ItemUseContext useContext;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _scrollRect = GetComponent<ScrollRect>();
            
            if (inventory == null)
                inventory = FindAnyObjectByType<InventoryCode>();

            Bus<SetItemContextEvent>.OnEvent += OnSetItemContext;

            BuildSlots();
            RefreshFromInventory();
            SetImmediate(false);
        }
        
        private void OnEnable()
        {

            Bus<ItemGainedEvent>.OnEvent += OnItemGained;
        }

        private void OnDisable()
        {
            Bus<ItemGainedEvent>.OnEvent -= OnItemGained;
        }

        private void OnDestroy()
        {
            Bus<SetItemContextEvent>.OnEvent -= OnSetItemContext;

        }

        private void OnItemGained(ItemGainedEvent e) => RefreshFromInventory();
        private void OnSetItemContext(SetItemContextEvent evt) => useContext = evt.context;

        public void BuildSlots()
        {
            if (inventory == null || itemParentTrm == null || itemPanelPrefab == null)
                return;
            
            for (int i = itemParentTrm.childCount - 1; i >= 0; i--)
                Destroy(itemParentTrm.GetChild(i).gameObject);

            _itemPanels.Clear();

            inventory.EnsureSlotsInitialized();

            for (int i = 0; i < inventory.slotCount; i++)
            {
                ItemPanelUI panel = Instantiate(itemPanelPrefab, itemParentTrm);
                panel.OnButtonClick += HandleButtonClick;
                // panel.Bind(null, 0);
                _itemPanels.Add(panel);
            }
        }

        public void RefreshFromInventory()
        {
            if (inventory == null) return;

            inventory.EnsureSlotsInitialized();
            var slots = inventory.inventorySlots;

            if (_itemPanels.Count != inventory.slotCount)
                BuildSlots();

            int count = Mathf.Min(_itemPanels.Count, slots.Length);

            for (int i = 0; i < count; i++)
            {
                var panel = _itemPanels[i];
                var stack = slots[i];

                ItemVisualDataSO visual = null;
                int amount = 0;

                if (stack.item != null && stack.amount > 0)
                {
                    visual = stack.item.visualData;
                    amount = stack.amount;
                }

                panel.Bind(i, visual, amount);
            }

            ItemPanelUI selectedPanel = null;
            if (_currentItem != null)
            {
                int idx = _currentItem.SlotIndex;
                if (idx >= 0 && idx < _itemPanels.Count)
                    selectedPanel = _itemPanels[idx];
                
                if (selectedPanel == null || selectedPanel.ItemData == null || selectedPanel.Amount <= 0)
                {
                    selectedPanel = null;
                    _currentItem = null;
                }
                else
                {
                    _currentItem = selectedPanel;
                }
            }

            for (int i = 0; i < _itemPanels.Count; i++)
            {
                _itemPanels[i].SetActiveItem(_itemPanels[i] == selectedPanel);
            }
        }

        private void HandleButtonClick(ItemPanelUI targetPanel)
        {
            if (_currentItem == targetPanel)
            {
                targetPanel.SetActiveItem(false);
                _currentItem = null;
                
                Bus<ItemVisualDataEvent>.Raise(new ItemVisualDataEvent(null));
                return;
            }

            quickSlot.ClearSelectSlots();
            foreach (ItemPanelUI panel in _itemPanels)
            {
                bool isSelected = (panel == targetPanel);
                panel.SetActiveItem(isSelected);
            }

            _currentItem = targetPanel;
            
            if (targetPanel.ItemData != null)
                Bus<ItemVisualDataEvent>.Raise(new ItemVisualDataEvent(targetPanel.ItemData));
        }
        
        public void OnRegisterQuickSlotButtonClick()
        {
            Bus<QuickSlotInOutEvent>.Raise(new QuickSlotInOutEvent(inventory, _currentItem));
            RefreshFromInventory();
        }

        private void SetImmediate(bool open)
        {
            _isOpened = open;

            //gameObject.SetActive(open);
        }
        
        public bool IsBlockingClose()
        {
            return deletePopup != null && deletePopup.IsInputEditing;
        }
        public void ClearSelectSlots()
        {
            foreach (ItemPanelUI panel in _itemPanels)
            {
                panel.SetActiveItem(false);
            }
            _currentItem = null;
        }
        public void ItemUse()
        {
            if (_currentItem == null)
                return;

            int slotIndex = _currentItem.SlotIndex;
            if (slotIndex < 0)
                return;

            int amount = inventory.GetAmountAtSlot(slotIndex);
            if (amount <= 0)
                return;
            
            bool useSuccess = TryApplyItemEffect(slotIndex);

            if (!useSuccess)
            {
                GuidePanelHelper.Instance.ShowGuidePanel(GuideTextType.CantUseItem);
                return;
            }

            bool removed = inventory.TryRemoveAtSlot(slotIndex, 1, compactAfter: true);
            if (!removed)
                return;

            //if (_currentItem != null)
            //    _currentItem.SetActiveItem(false);

            //_currentItem = null;
            RefreshFromInventory();
            Bus<ItemVisualDataEvent>.Raise(new ItemVisualDataEvent(inventory.inventorySlots[slotIndex].item?.visualData));
        }
        
        private bool TryApplyItemEffect(int slotIndex)
        {
            var stack = inventory.inventorySlots[slotIndex];
            if (stack.item == null)
                return false;

            if(stack.item.useFunction == null)
            {
                Debug.Log($"{stack.item.name} 아이탬은 사용 기능이 없어요");
                return false;
            }

            if (useContext.user == null)
            {
                Debug.Log("아이탬 사용자가 useContext에 안들어 왔어요");
                return false;
            }

            if (!stack.item.useFunction.Use(useContext))
                return false;

            Debug.Log($"{stack.item.name} 사용 성공");

            return true;
        }

        public void ItemDelete()
        {
            if (_currentItem == null)
                return;

            if (deletePopup == null)
                return;

            int slot = _currentItem.SlotIndex;
            int max = inventory.GetAmountAtSlot(slot);
            if (max <= 0)
                return;

            deletePopup.Show(
                visual: _currentItem.ItemData,
                slotIndex: slot,
                maxCount: max,
                onConfirm: ConfirmDelete
            );
        }

        private void ConfirmDelete(int slotIndex, int count)
        {
            bool ok = inventory.TryRemoveAtSlot(slotIndex, count, compactAfter: true);
            if (!ok) return;
            
            if (_currentItem != null)
                _currentItem.SetActiveItem(false);

            _currentItem = null;
            RefreshFromInventory();
            Bus<ItemVisualDataEvent>.Raise(new ItemVisualDataEvent(null));
        }
 
        public void OnScroll(PointerEventData eventData)
        {
            if (_scrollRect != null)
                _scrollRect.velocity = Vector2.zero;
        }
    }
}