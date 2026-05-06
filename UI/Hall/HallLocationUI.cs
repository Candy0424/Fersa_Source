using System;
using PSW.Code.EventBus;
using UnityEngine;
using YIS.Code.Events;

namespace Work.YIS.Code.UI.Hall
{
    public class HallLocationUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        
        private void Awake()
        {
            Bus<HallEnterUIEvent>.OnEvent += HandleEnterUI;
            
            SetActive(false);
        }
        
        private void OnDestroy()
        {
            Bus<HallEnterUIEvent>.OnEvent -= HandleEnterUI;
        }

        private void HandleEnterUI(HallEnterUIEvent evt)
        {
            SetActive(evt.IsActive);
        }

        private void SetActive(bool isActive)
        {
            int alphaValue = isActive ? 1 : 0;
            bool activeValue = isActive;
            
            canvasGroup.interactable = activeValue;
            canvasGroup.alpha = alphaValue;
            canvasGroup.blocksRaycasts = isActive;
        }
    }
}