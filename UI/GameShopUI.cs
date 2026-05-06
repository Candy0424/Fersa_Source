using PSB.Code.BattleCode.Entities;
using PSB_Lib.Dependencies;
using PSW.Code.BaseSystem;
using PSW.Code.Battle;
using PSW.Code.EventBus;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using YIS.Code.CoreSystem.Interfaces;
using YIS.Code.Defines;
using YIS.Code.Events;
using YIS.Code.Items;

namespace YIS.Code.Combat
{
    public class GameShopUI : BaseOnOffAnimationUI, IBaseSystemUI
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [Inject] private IShopService _service;

        [SerializeField] private EntityHealth health;
        [SerializeField] private HpUI_Controller hpUI;

        [SerializeField] private Transform itemRoot;
        [SerializeField] private Transform skillRoot;
        [SerializeField] private Transform hpRoot;
        [SerializeField] private Transform detailRoot;
        
        
        private List<ShopItemUI> _shopItems;
        private List<ShopItemUI> _shopSkills;
        private List<ShopItemUI> _shopHps;
        
        private Dictionary<DetailUIType, DetailBtnUI> _detailDatas;
        
        public override void Awake()
        {
            base.Awake();
            hpUI.Init(health.CurrentHealth, health.MaxHealth);
            _detailDatas = new Dictionary<DetailUIType, DetailBtnUI>();
            _shopItems = itemRoot.GetComponentsInChildren<ShopItemUI>().ToList();
            _shopSkills = skillRoot.GetComponentsInChildren<ShopItemUI>().ToList();
            _shopHps = hpRoot.GetComponentsInChildren<ShopItemUI>().ToList(); 
            _detailDatas = detailRoot.GetComponentsInChildren<DetailBtnUI>().ToDictionary(item => item.DetailType, item => item);

            health.OnHealthChangeEvent += SetShopHpUi;
            
        }
        private void Start()
        {
            RegisterBtnEvent();
            VisualLoad();

            SetActive((false));
        }


        private void SetShopHpUi(float currentHealth, float maxHealth)
        {
            hpUI.ChangeCurrentHp(currentHealth);
        }

        public void SetActive(bool isActive)
        {
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.interactable = isActive;
            canvasGroup.blocksRaycasts = isActive;
        }

        private void RegisterBtnEvent()
        {
            ResetEvent();
            
            List<ShopItemDataSO> itemList = _service.LoadItems();
            List<ShopItemDataSO> skillList = _service.LoadSkills();
            List<ShopItemDataSO> hpList = _service.LoadHpItems();
            List<DetailDataSO> detailList = _service.LoadDetailData();
            
            for (int i = 0; i < itemList.Count; i++)
            {
                ShopItemUI ui = _shopItems[i];
                Button btn = _shopItems[i].Btn;
                btn.onClick.AddListener(() =>
                {
                    if (SpendItem(ui.ItemDataSO))
                    {
                        btn.onClick.RemoveAllListeners();
                        ui.SpendItem();
                    }
                });
            }

            for (int i = 0; i < skillList.Count; i++)
            {
                ShopItemUI ui = _shopSkills[i];
                Button btn = _shopSkills[i].Btn;
                btn.onClick.AddListener(() =>
                {
                    if (SpendItem(ui.ItemDataSO))
                    {
                        btn.onClick.RemoveAllListeners();
                        ui.SpendItem();
                    }
                });
            }

            for (int i = 0; i < hpList.Count; i++)
            {
                ShopItemUI ui = _shopHps[i];
                Button btn = _shopHps[i].Btn;
                btn.onClick.AddListener(() => SpendItem(ui.ItemDataSO));
            }

            for (int i = 0; i < detailList.Count; i++)
            {
                var detail = detailList[i];

                DetailBtnUI ui = _detailDatas[detail.detailType];
                Button btn = _detailDatas[detail.detailType].Btn;
                btn.onClick.AddListener( () => UseDetail(ui));
            }
        }

        private void ResetEvent()
        {
            foreach (ShopItemUI ui in _shopItems) ui.Btn.onClick.RemoveAllListeners();
            foreach (ShopItemUI ui in _shopSkills) ui.Btn.onClick.RemoveAllListeners();
            foreach (ShopItemUI ui in _shopHps) ui.Btn.onClick.RemoveAllListeners();
            foreach (DetailBtnUI ui in _detailDatas.Values) ui.Btn.onClick.RemoveAllListeners();
        }
        
        private void OnDisable()
        {
            Debug.Log("꺼짐");    
            Bus<SellItemDataEvent>.OnEvent -= HandleSellItem;
            Bus<SellShopUIActiveEvent>.OnEvent -= HandleUIActiveEvent;
            Bus<SellShopUIActiveEvent>.Raise(new SellShopUIActiveEvent(false));
            Bus<DescUIActiveEvent>.Raise(new DescUIActiveEvent(false));
        }

        private void OnDestroy()
        {

            Bus<SellItemDataEvent>.OnEvent -= HandleSellItem;
            Bus<SellShopUIActiveEvent>.OnEvent -= HandleUIActiveEvent;

            health.OnHealthChangeEvent -= SetShopHpUi;
            ResetEvent();
        }

        private void VisualLoad()
        {
            List<ShopItemDataSO> itemList = _service.LoadItems();
            List<ShopItemDataSO> skillList = _service.LoadSkills();
            List<ShopItemDataSO> hpList = _service.LoadHpItems();
            List<DetailDataSO> detailList = _service.LoadDetailData();

            for (int i = 0; i < itemList.Count; i++)
            {
                var item = itemList[i];
                
                ShopItemUI ui = _shopItems[i];
                ui.Initialize(item);
            }

            for (int i = 0; i < skillList.Count; i++)
            {
                var skill = skillList[i];
                
                ShopItemUI ui = _shopSkills[i];
                ui.Initialize(skill);
            }

            for (int i = 0; i < hpList.Count; i++)
            {
                var hp = hpList[i];
                
                ShopItemUI ui = _shopHps[i];
                ui.Initialize(hp);
            }

            for (int i = 0; i < detailList.Count; i++)
            {
                var detail = detailList[i];

                DetailBtnUI ui = _detailDatas[detail.detailType];
                ui.InitData(detail);
            }
        }

        private void UseDetail(DetailBtnUI detail)
        {
            switch (detail.DetailType)
            {
                case DetailUIType.Quit:
                    Quit();
                    break;
                case DetailUIType.Restock:
                    Restock(detail);
                    break;
                case DetailUIType.Sell:
                    Sell();
                    break;
                default:
                    break;
            }
        }
        private void Quit()
        {
            PopUp();
        }

        private void Sell()
        {
            Bus<SellItemDataEvent>.OnEvent += HandleSellItem;
            Bus<SellShopUIActiveEvent>.OnEvent += HandleUIActiveEvent;

            Bus<SellShopUIActiveEvent>.Raise(new SellShopUIActiveEvent(true));
        }

        private void HandleUIActiveEvent(SellShopUIActiveEvent evt)
        {
            if (evt.IsActive) return;
            
            Bus<SellItemDataEvent>.OnEvent -= HandleSellItem;
            Bus<SellShopUIActiveEvent>.OnEvent -= HandleUIActiveEvent;
        }

        private void HandleSellItem(SellItemDataEvent evt)
        {
            SellItem(evt.ItemSellData, evt.Amount, evt.SlotNumber);
        }

        private void Restock(DetailBtnUI detail)
        {
            if (!_service.RestoreItem((int)detail.Price))
            {
                Debug.Log("<color=red>재입고 실패!</color>");
                return;
            }
            Debug.Log("<color=green>재입고 성공!</color>");
            ResetItems();
            VisualLoad();
        }

        private void ResetItems()
        {
            foreach (ShopItemUI ui in _shopItems) ui.ResetItem(); 
            foreach (ShopItemUI ui in _shopSkills) ui.ResetItem();
            
            RegisterBtnEvent();
        }

        private bool SpendItem(ShopItemDataSO item)
        {
            if (!_service.SpendItem(item))
            {
                Debug.Log("<color=red>구매 실패!</color>");
                return false;
            }
            Debug.Log("<color=green>구매 성공!</color>");
            return true;
        }

        private bool SellItem(ItemSellDataSO item, int amount, int slotNumber)
        {
            if (!_service.SellItem(item, amount, slotNumber))
            {
                Debug.Log("<color=red>판매 실패!</color>");
                return false;
            }
            Debug.Log("<color=green>판매 성공!</color>");
            return true;
        }

        public void DataInit()
        {

        }

        public void DataDestroy()
        {
            foreach (ShopItemUI ui in _shopItems) ui.SetEndShop();
            foreach (ShopItemUI ui in _shopSkills) ui.SetEndShop();
            foreach (ShopItemUI ui in _shopHps) ui.SetEndShop();

            Bus<SellShopUIActiveEvent>.Raise(new SellShopUIActiveEvent(false));
        }
    }
}