using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YIS.Code.Items;

namespace YIS.Code.Combat
{
    public class SellItemPanelUI : MonoBehaviour
    {
        [Header("Component")]
        [field: SerializeField] public Button Btn { get; private set; }
        [SerializeField] private Image itemImage;
        [SerializeField] private SpriteData buttonPopData;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private TextMeshProUGUI goldText;

        [Header("Tween")]
        [SerializeField] private float duration = 0.12f;

        [field: SerializeField] public ItemVisualDataSO ItemData { get; private set; }
        public int Amount { get; private set; }

        private bool _isDisable = true;
        public bool IsDisable => _isDisable;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            Btn = GetComponentInChildren<Button>();
            Btn.image.sprite = buttonPopData.GetSprite(false);
            _isDisable = true;
            SetEmpty();
        }

        public void EnableData()
        {
            _isDisable = true;
            gameObject.SetActive(true);
        }

        public void SetData(ItemVisualDataSO data, int amount, int gold)
        {
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
            if (goldText != null)
                goldText.SetText(gold.ToString());
        }

        public void AddEvent(UnityAction action)
        {
            Btn.onClick.AddListener(action);
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

        public void SetActiveItem(bool isActive, bool instant = false)
        {
            Btn.image.sprite = buttonPopData.GetSprite(isActive);
        }

        public void ResetData()
        {
            ItemData = null;
            SetEmpty();
            SetActiveItem(false, true);
            Btn.onClick.RemoveAllListeners();
            _isDisable = true;
            gameObject.SetActive(false);
        }
    }
}