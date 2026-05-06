using PSW.Code.EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YIS.Code.Events;

namespace YIS.Code.Combat
{
    public class ItemDetailPanelUI : MonoBehaviour
    {
        [SerializeField] private Image detailImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descText;

        private void Awake()
        {
            Bus<ItemVisualDataEvent>.OnEvent += HandleVisualDataEvent;
            nameText.SetText(string.Empty);
            descText.SetText(string.Empty);
        }

        private void OnDestroy()
        {
            Bus<ItemVisualDataEvent>.OnEvent -= HandleVisualDataEvent;
        }

        private void HandleVisualDataEvent(ItemVisualDataEvent evt)
        {
            if (evt.ItemVisualData == null)
            {
                detailImage.sprite = null;
                detailImage.enabled = false;
                nameText.SetText("");
                descText.SetText("");

                return;
            }
            
            Debug.Log($"{evt.ItemVisualData}을 클릭함.");
            detailImage.enabled = true;
            detailImage.sprite = evt.ItemVisualData.icon;
            nameText.SetText(evt.ItemVisualData.uiName);
            descText.SetText(evt.ItemVisualData.itemDescription);
        }
        
    }
}