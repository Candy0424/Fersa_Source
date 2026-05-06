using UnityEngine;
using YIS.Code.Defines;
using YIS.Code.Skills;

namespace YIS.Code.Items
{
    [CreateAssetMenu(fileName = "New ShopItemData", menuName = "SO/Item/ShopItemData", order = 0)]
    public class ShopItemDataSO : ScriptableObject
    {
        [Tooltip("ID 규칙 : 스킬들 10번부터 / 아이템들 1000번부터")] public int id;
        public int itemPrice;
        [HideInInspector] public ShopItemType shopItemType;

        [HideInInspector] public ItemDataSO itemData;
        [HideInInspector] public SkillDataSO skillData;
        [HideInInspector] public HpDataSO hpData;
    }
}