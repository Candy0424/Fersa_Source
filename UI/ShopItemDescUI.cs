using PSW.Code.EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YIS.Code.Events;
using YIS.Code.Items;

namespace YIS.Code.Combat
{
    public class ShopItemDescUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform fitterRoot;
        
        [SerializeField] private Image visual;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descText;

        private void Awake()
        {
            RegisterEvent();
            SetActive(false);
        }

        private void RegisterEvent()
        {
            Bus<DescUIActiveEvent>.OnEvent += HandleActive;
            Bus<DescUIEvent>.OnEvent += HandleDesc;
        }

        private void HandleDesc(DescUIEvent evt)
        {
            SetData(evt.Data);
        }

        private void HandleActive(DescUIActiveEvent evt)
        {
            SetActive(evt.IsActive);
        }

        private void OnDestroy()
        {
            ResetEvent();
        }

        private void ResetEvent()
        {
            Bus<DescUIActiveEvent>.OnEvent -= HandleActive;
            Bus<DescUIEvent>.OnEvent -= HandleDesc;
        }

        private void SetActive(bool isActive)
        {
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.interactable = isActive;
        }

        private void SetData(ItemVisualDataSO data)
        {
            visual.sprite = data.icon;
            titleText.SetText(data.uiName);
            descText.SetText(data.itemDescription);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(fitterRoot);
        }
    }
}