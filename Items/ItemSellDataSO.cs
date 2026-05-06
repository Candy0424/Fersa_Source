using UnityEngine;
using YIS.Code.Defines;

namespace YIS.Code.Items
{
    [CreateAssetMenu(fileName = "New ItemSellData", menuName = "SO/Item/SellData", order = 0)]
    public class ItemSellDataSO : ScriptableObject
    {
        public ItemType cacheType;

        public int sellValue;
    }
}