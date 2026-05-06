using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YIS.Code.Items;

namespace YIS.Code.Combat
{
    public class ItemPanelUI : MonoBehaviour
    {
        [Header("Component")]
        [SerializeField] private Button button;
        [SerializeField] private Image itemImage;
        [SerializeField] private Image selectToBox;
        [SerializeField] private TextMeshProUGUI amountText;

        [Header("Tween")]
        [SerializeField] private float duration = 0.12f;

        [field: SerializeField] public ItemVisualDataSO ItemData { get; private set; }
        [field: SerializeField] public int SlotIndex { get; private set; } = -1;
        public int Amount { get; private set; }

        public event Action<ItemPanelUI> OnButtonClick;

        private Sequence _seq;
        private bool _isSelected;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (button != null)
                button.onClick.AddListener(HandleClick);

            if (selectToBox != null)
            {
                var c = selectToBox.color;
                c.a = 0f;
                selectToBox.color = c;
                selectToBox.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            }

            SetEmpty();
        }

        public void Bind(int slotIndex, ItemVisualDataSO data, int amount)
        {
            SlotIndex = slotIndex;
            ItemData = data;
            Amount = amount;

            if (ItemData == null || Amount <= 0)
            {
                SetEmpty();
                return;
            }

            if (itemImage != null)
            {
                itemImage.enabled = true;
                itemImage.sprite = ItemData.icon;
            }

            if (amountText != null)
                amountText.SetText(Amount.ToString());
        }

        private void SetEmpty()
        {
            ItemData = null;
            Amount = 0;

            if (itemImage != null)
            {
                itemImage.sprite = null;
                itemImage.enabled = false;
            }

            if (amountText != null)
                amountText.SetText("");

            SetActiveItem(false, instant: true);
        }

        private void HandleClick()
        {
            if (ItemData == null || Amount <= 0) return;
            OnButtonClick?.Invoke(this);
        }

        public void SetActiveItem(bool isActive, bool instant = false)
        {
            if (selectToBox == null) return;
            
            if (_isSelected == isActive && !instant)
                return;

            _isSelected = isActive;
            
            _seq?.Kill();
            selectToBox.DOKill();
            selectToBox.transform.DOKill();

            float targetAlpha = isActive ? 1f : 0f;
            float targetScale = isActive ? 1.1f : 1.4f;

            if (instant)
            {
                var c = selectToBox.color;
                c.a = targetAlpha;
                selectToBox.color = c;
                selectToBox.transform.localScale = Vector3.one * targetScale;
                return;
            }

            _seq = DOTween.Sequence()
                .Append(selectToBox.transform.DOScale(targetScale * 0.98f, duration).SetEase(Ease.Linear))
                .Join(selectToBox.DOFade(targetAlpha, duration))
                .Append(selectToBox.transform.DOScale(targetScale, duration).SetEase(Ease.Linear))
                .SetUpdate(true);
        }
        
    }
}
