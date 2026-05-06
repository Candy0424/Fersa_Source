using CSH.Scripts.Attributes;
using UnityEngine;
using YIS.Code.Defines;

namespace YIS.Code.Items
{
    [CreateAssetMenu(fileName = "New ItemVisualData", menuName = "SO/Item/ItemVisualData", order = 0)]
    public class ItemVisualDataSO : ScriptableObject
    {
        public string itemName; // SO 이름
        public string uiName; // UI에 사용할 이름
        [TextArea] public string itemDescription; // 아이템 설명
        
        [SpritePreview] public Sprite icon;

        private string _prevItemName;
            
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(itemName))
            { 
                Debug.LogError( "아이템 이름은 공백일 수 없습니다!");
                itemName = _prevItemName;
                return;
            }
            _prevItemName = itemName;
        }
    }
}