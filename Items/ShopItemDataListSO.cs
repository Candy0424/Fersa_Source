using UnityEngine;

namespace YIS.Code.Items
{
    [CreateAssetMenu(fileName = "New ShopItemDataList", menuName = "SO/Item/ShopItemDataList", order = 0)]
    public class ShopItemDataListSO : ScriptableObject
    {
        public ShopItemDataSO[] shopItemDataList;
    }
}