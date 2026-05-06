using CSH.Scripts.Items;
using UnityEngine;
using UnityEngine.Serialization;
using YIS.Code.Defines;

namespace YIS.Code.Items
{
    [CreateAssetMenu(fileName = "New ItemData", menuName = "SO/Item/ItemData", order = 0)]
    public class ItemDataSO : ScriptableObject
    {
        public int itemId;
        public string itemName;
        public ItemType itemType;
        public GameObject itemPrefab;
        public ItemVisualDataSO visualData;
        public ItemSellDataSO sellData;
        public ItemUseFunctionSO useFunction;

        private void OnValidate()
        {
            name = itemName;
        }
    }
}