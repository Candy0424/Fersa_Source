using System.Collections.Generic;
using System.Linq;
using PSB_Lib.Dependencies;
using PSB.Code.BattleCode.Events;
using PSB.Code.BattleCode.UIs.BossShopUI;
using PSB.Code.CoreSystem.Events;
using PSB.Code.CoreSystem.SaveSystem;
using PSB.Code.CoreSystem.SaveSystem.BossShop;
using PSW.Code.EventBus;
using UnityEngine;
using YIS.Code.CoreSystem.Interfaces;
using YIS.Code.Defines;
using YIS.Code.Effects;
using YIS.Code.Events;
using YIS.Code.Items;
using Random = UnityEngine.Random;

namespace YIS.Code.CoreSystem
{
    public class ShopService : MonoBehaviour, IShopService, IDependencyProvider
    {
        [SerializeField] private ShopItemDataListSO shopItemDataList;
        [SerializeField] private BossItemListSO bossItemDataList;

        [SerializeField] private int maxItemPickupNum;
        [SerializeField] private int maxSkillPickupNum;
        
        [Inject] private BossUnlockRepository _unlockRepository;

        private Dictionary<int, ShopItemDataSO> _items;
        private Dictionary<int, ShopItemDataSO> _skills;

        [SerializeField] private List<ShopItemDataSO> pickUpHpTable;
        [SerializeField] private List<DetailDataSO> detailUiTable;

        private List<ShopItemDataSO> _pickUpItemTable;
        private List<ShopItemDataSO> _pickUpSkillTable;

        private List<int> _itemKeyTable;
        private List<int> _skillKeyTable;

        [Provide]
        public IShopService Provide() => this;

        
        private void Awake()
        {
            _items = new Dictionary<int, ShopItemDataSO>();
            _skills = new Dictionary<int, ShopItemDataSO>();
            _pickUpItemTable = new List<ShopItemDataSO>();
            _pickUpSkillTable = new List<ShopItemDataSO>();

            Initialize();
        }
        
        private void OnEnable()
        {
            Bus<BossShopUnlockEvent>.OnEvent += HandleBossUnlock;
        }

        private void OnDisable()
        {
            Bus<BossShopUnlockEvent>.OnEvent -= HandleBossUnlock;
        }

        private void HandleBossUnlock(BossShopUnlockEvent evt)
        {
            RebuildCatalogOnly();
        }

        private void Initialize()
        {
            RebuildCatalogOnly();
            InitItems();
        }

        private void RebuildCatalogOnly()
        {
            _items.Clear();
            _skills.Clear();
            _pickUpItemTable.Clear();
            _pickUpSkillTable.Clear();

            AddCatalog(shopItemDataList.shopItemDataList);

            if (bossItemDataList != null && bossItemDataList.shopItemDataList != null)
            {
                foreach (var unlockItem in bossItemDataList.shopItemDataList)
                {
                    if (unlockItem == null) continue;
                    if (_unlockRepository == null) continue;
                    if (!_unlockRepository.IsUnlocked(unlockItem.id)) continue;
                    if (unlockItem.shopItemData == null) continue;

                    AddItemToCatalog(unlockItem.shopItemData);
                }
            }

            _itemKeyTable = _items.Keys.ToList();
            _skillKeyTable = _skills.Keys.ToList();
        }

        private void AddCatalog(IEnumerable<ShopItemDataSO> catalog)
        {
            if (catalog == null) return;

            foreach (var item in catalog)
            {
                AddItemToCatalog(item);
            }
        }

        private void AddItemToCatalog(ShopItemDataSO item)
        {
            if (item == null) return;

            switch (item.shopItemType)
            {
                case ShopItemType.Item:
                    if (!_items.ContainsKey(item.id))
                        _items.Add(item.id, item);
                    break;

                case ShopItemType.Skill:
                    if (!_skills.ContainsKey(item.id))
                        _skills.Add(item.id, item);
                    break;
            }
        }

        public void InitItems()
        {
            for (int i = 0; i < maxItemPickupNum; i++)
            {
                if (_itemKeyTable.Count <= 0) break;

                int index = Random.Range(0, _itemKeyTable.Count);
                int itemKey = _itemKeyTable[index];

                _pickUpItemTable.Add(_items[itemKey]);

                _itemKeyTable[index] = _itemKeyTable[^1];
                _itemKeyTable.RemoveAt(_itemKeyTable.Count - 1);
            }

            for (int i = 0; i < maxSkillPickupNum; i++)
            {
                if (_skillKeyTable.Count <= 0) break;

                int index = Random.Range(0, _skillKeyTable.Count);
                int skillKey = _skillKeyTable[index];

                _pickUpSkillTable.Add(_skills[skillKey]);

                _skillKeyTable[index] = _skillKeyTable[^1];
                _skillKeyTable.RemoveAt(_skillKeyTable.Count - 1);
            }
        }

        private void ResetItem()
        {
            _itemKeyTable = _items.Keys.ToList();
            _skillKeyTable = _skills.Keys.ToList();

            _pickUpItemTable.Clear();
            _pickUpSkillTable.Clear();
        }

        public List<ShopItemDataSO> LoadItems() => _pickUpItemTable;
        public List<ShopItemDataSO> LoadSkills() => _pickUpSkillTable;
        public List<ShopItemDataSO> LoadHpItems() => pickUpHpTable;
        public List<DetailDataSO> LoadDetailData() => detailUiTable;

        public bool SpendItem(ShopItemDataSO item)
        {
            if (item == null) return false;
            if (!CurrencyContainer.Spend(ItemType.Coin, item.itemPrice)) return false;

            switch (item.shopItemType)
            {
                case ShopItemType.Item:
                    Bus<SpendItemEvent>.Raise(new SpendItemEvent(item.itemData));
                    break;

                case ShopItemType.Skill:
                    Bus<SpendSkillEvent>.Raise(new SpendSkillEvent(item.skillData));
                    break;

                case ShopItemType.Hp:
                    Bus<HealRequest>.Raise(new HealRequest(item.hpData.value, HealMode.MaxPercent));
                    break;
            }

            return true;
        }

        public bool RestoreItem(int price)
        {
            if (!CurrencyContainer.Spend(ItemType.Coin, price)) return false;

            RebuildCatalogOnly();
            ResetItem();
            InitItems();

            return true;
        }

        public bool SellItem(ItemSellDataSO item, int amount, int slotNumber)
        {
            if (item.cacheType == ItemType.None)
            {
                Debug.Log($"<color=red>잘못된 캐시 타입</color> {item.cacheType}");
                return false;
            }

            CurrencyContainer.Add(item.cacheType, item.sellValue * amount);
            Bus<SellInvenItemEvent>.Raise(new SellInvenItemEvent(slotNumber, amount));

            return true;
        }
        
    }
}