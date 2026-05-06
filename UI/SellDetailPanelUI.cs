using PSB.Code.CoreSystem.SaveSystem;
using PSW.Code.EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YIS.Code.Events;
using YIS.Code.Items;

namespace YIS.Code.Combat
{
    public class SellDetailPanelUI : MonoBehaviour
    {
        [Header("Images")]
        [SerializeField] private Image visualImage;
        [SerializeField] private Sprite defaultSprite;
        
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descText;
        [SerializeField] private TextMeshProUGUI valueText;
        
        [Header("Buttons")]
        [SerializeField] private Button sellBtn;
        [SerializeField] private Button cancelBtn;
        [SerializeField] private Button minusBtn;
        [SerializeField] private Button plusBtn;

        [Header("Sliders")] 
        [SerializeField] private Slider valueSlider;

        private int _curItemId;
        private int _curSlotNumber;
        
        private void Awake()
        {
            RegisterEvent();
        }

        private void OnEnable()
        {
            SetEmpty();
        }

        private void RegisterEvent()
        {
            Bus<SellDataUpdateEvent>.OnEvent += DataUpdate;
            
            valueSlider.onValueChanged.AddListener(HandleValueChange);
            plusBtn.onClick.AddListener(HandlePlusBtnClick);
            minusBtn.onClick.AddListener(HandleMinusBtnClick);
            cancelBtn.onClick.AddListener(HandleCancelBtnClick);
            sellBtn.onClick.AddListener(HandleSellBtnClick);
        }

        private void HandleSellBtnClick()
        {
            InvenData itemData = new InvenData()
            {
                slotNumber = _curSlotNumber,
                itemId = _curItemId,
                amount = (int)valueSlider.value
            };
            
            if (itemData.amount <= 0)
            {
                Debug.Log("<color=yellow>최소한 1개는 지정해야 합니다.</color>");
                return;
            }
            Bus<SellInvenItemDataEvent>.Raise(new SellInvenItemDataEvent(itemData));
            SetEmpty();
        }

        private void HandleCancelBtnClick()
        {
            //SetEmpty();
        }

        private void HandleMinusBtnClick()
        {
            valueSlider.value -= 1;
            HandleValueChange(valueSlider.value);
        }

        private void HandlePlusBtnClick()
        {
            valueSlider.value += 1;
            HandleValueChange(valueSlider.value);
        }

        private void DataUpdate(SellDataUpdateEvent evt)
        {
            SetData(evt.VisualData, evt.InvenItemData);
        }

        private void SetData(ItemVisualDataSO item, InvenData invenItemData)
        {
            if (visualImage == null) return;

            visualImage.sprite = item.icon;
            
            nameText.SetText(item.uiName);
            descText.SetText(item.itemDescription);

            valueSlider.minValue = 0;
            valueSlider.value = 1;
            valueSlider.maxValue = invenItemData.amount;

            _curItemId = invenItemData.itemId;
            _curSlotNumber = invenItemData.slotNumber;
        }

        private void SetEmpty()
        {
            Debug.Log($"이미지지 {visualImage}");
            visualImage.sprite = defaultSprite;
            
            nameText.SetText("");
            descText.SetText("");

            valueSlider.minValue = 0;
            valueSlider.value = 0;
            valueText.SetText("0");
            valueSlider.maxValue = 0;
            
            _curItemId = -1;
            _curSlotNumber = -1;
        }

        private void HandleValueChange(float value) => valueText.SetText(((int)value).ToString());
    }
}