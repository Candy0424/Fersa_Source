using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YIS.Code.Defines;
using YIS.Code.Items;

namespace YIS.Code.Combat
{
    public class DetailBtnUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [field: SerializeField] public DetailUIType DetailType { get; private set; }
        [field: SerializeField] public DetailDataSO DetailData { get; private set; }
        [field: SerializeField] public Button Btn { get; private set; }
        [SerializeField] private TextMeshProUGUI uiName;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Image icon;
        [SerializeField] private Image goldIcon;
        [SerializeField] private GameObject selectBox;

        private uint _price;

        public uint Price => _price;

        public void InitData(DetailDataSO detailData)
        {
            DetailData = detailData;
            SetDetailData();
            selectBox?.SetActive(false);
            _price = detailData.basePrice;
        }
        
        private void SetDetailData()
        {
            _price = DetailData.basePrice;
            uiName.SetText(DetailData.detailName);
            icon.sprite = DetailData.detailIcon;

            if (DetailData.useGold)
            {
                //goldIcon.sprite = DetailData.cacheIcon;
                //priceText.SetText(_price.ToString());
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            selectBox.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            selectBox.SetActive(false);
        }
    }
}