using DG.Tweening;
using PSW.Code.Deck;
using PSW.Code.Dial;
using PSW.Code.EventBus;
using PSW.Code.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YIS.Code.Defines;
using YIS.Code.Events;
using YIS.Code.Items;

namespace YIS.Code.Combat
{
    public class ShopItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [field: SerializeField] public ShopItemDataSO ItemDataSO { get; private set; }

        [field: SerializeField] public Button Btn { get; private set; }
        
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemDescriptionText;
        [SerializeField] private TextMeshProUGUI itemPriceText;
        [SerializeField] private Image itemIcon;
        [SerializeField] private Image currencyIcon;
        [SerializeField] private Image colorOutLine;

        [SerializeField] private Sprite coinIcon;
        [SerializeField] private Sprite bossCoinIcon;

        [SerializeField] private Material outLineMap;
        [SerializeField] private string setValue = "_BloomPower";
        [SerializeField] private float popTime = 0.25f;
        [SerializeField] private float popUpValue = 10;

        [SerializeField] private Material popMap;
        [SerializeField] private Graphic targetGraphic;
        [SerializeField] private Mask mask;
        [SerializeField] private string setPopValue = "_UpValue";
        [SerializeField] private float popUiTime = 0.2f;
        [SerializeField] private float popUiValue = 2;

        [SerializeField] private PurchaseEffect effect;
        [SerializeField] private SkillCircleDataListSO colorDataSo;
        private bool _isSpend = false;
        private Material _newPopMap;

        public void Initialize(ShopItemDataSO itemDataSO)
        {
            ItemDataSO = itemDataSO;
           
            if(popMap != null)
                _newPopMap = Instantiate(popMap);

            if(mask != null)
                mask.enabled = false;

            SetData(itemDataSO);
            gameObject.SetActive(!_isSpend);
            outLineMap.SetFloat(setValue, 0);
        }

        private void SetData(ShopItemDataSO itemDataSO)
        {
            Btn.image.material = null;
            ItemDataSO = itemDataSO;
            effect?.ResetEffect();

            if (mask != null)
                mask.enabled = false;

            switch (itemDataSO.shopItemType)
            {
                case ShopItemType.Item:
                    itemIcon.sprite = itemDataSO.itemData.visualData.icon;
                    itemNameText.SetText(itemDataSO.itemData.visualData.uiName);
                    // itemDescriptionText.SetText(itemDataSO.visualData.itemDescription);
                    itemPriceText.SetText(itemDataSO.itemPrice.ToString());
                    break;
                case ShopItemType.Skill:
                    itemIcon.sprite = itemDataSO.skillData.visualData.icon;
                    itemNameText.SetText(itemDataSO.skillData.visualData.uiName);
                    // itemDescriptionText.SetText(itemDataSO.visualData.itemDescription);
                    itemPriceText.SetText(itemDataSO.itemPrice.ToString());

                    if (colorOutLine == null || colorDataSo == null)
                        break;

                    colorOutLine.color = colorDataSo.GetOutLineColor(itemDataSO.skillData.grade);
                    break;
                case ShopItemType.Hp:
                    itemIcon.sprite = itemDataSO.hpData.visualData.icon;
                    itemNameText.SetText(itemDataSO.hpData.visualData.uiName);
                    itemPriceText.SetText(itemDataSO.itemPrice.ToString());
                    break;
            }
            SetCurrencyIcon(ItemType.Coin);
        }
        
        private void SetCurrencyIcon(ItemType currencyType)
        {
            if (currencyIcon == null) return;

            switch (currencyType)
            {
                case ItemType.Coin:
                    currencyIcon.sprite = coinIcon;
                    effect.InitEffect(coinIcon);
                    currencyIcon.gameObject.SetActive(coinIcon != null);
                    break;

                case ItemType.BossCoin:
                    currencyIcon.sprite = bossCoinIcon;
                    effect.InitEffect(bossCoinIcon);
                    currencyIcon.gameObject.SetActive(bossCoinIcon != null);
                    break;

                default:
                    currencyIcon.sprite = null;
                    currencyIcon.gameObject.SetActive(false);
                    break;
            }
        }


        public void SpendItem()
        {
            _isSpend = true;

            if (mask != null)
                mask.enabled = true;

            effect?.PlayEffect(itemPriceText.text);
            targetGraphic.material = _newPopMap;
            targetGraphic.materialForRendering.DOFloat(popUiValue, setPopValue, popUiTime)
                .OnPlay(() => targetGraphic.SetMaterialDirty())
                .OnComplete(() =>
                {
                    _newPopMap.SetFloat(setPopValue, 0);
                    gameObject.SetActive(false);
                });
        }

        public void ResetItem()
        {
            _isSpend = false;
            outLineMap.SetFloat(setValue, 0);
            gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            outLineMap.SetFloat(setValue, 0);
        }   

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(Btn.image.material != _newPopMap)
            {
                outLineMap.DOKill();
                Btn.image.material = outLineMap;
                outLineMap.DOFloat(popUpValue, setValue, popTime);
            }

            Bus<DescUIActiveEvent>.Raise(new DescUIActiveEvent(true));
            switch (ItemDataSO.shopItemType)
            {
                case ShopItemType.Item:
                    Bus<DescUIEvent>.Raise(new DescUIEvent(ItemDataSO.itemData.visualData));
                    break;
                case ShopItemType.Skill:
                    Bus<DescUIEvent>.Raise(new DescUIEvent(ItemDataSO.skillData.visualData));
                    break;
                case ShopItemType.Hp:
                    Bus<DescUIEvent>.Raise(new DescUIEvent(ItemDataSO.hpData.visualData));
                    break;
                default:
                    break;
            }
        }

        public void SetEndShop()
        {
            Btn.image.material = null;
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            if(Btn.image.material == outLineMap)
            {
                outLineMap.DOFloat(0, setValue, popTime)
                .OnComplete(() => Btn.image.material = null)
                .OnKill(() => Btn.image.material = null);
            }
            Bus<DescUIActiveEvent>.Raise(new DescUIActiveEvent(false));
        }
    }
}