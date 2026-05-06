using UnityEngine;
using YIS.Code.Defines;

namespace YIS.Code.Items
{
    [CreateAssetMenu(fileName = "New Details", menuName = "SO/Detail/UIDetailData", order = 0)]
    public class DetailDataSO : ScriptableObject
    {
        public DetailUIType detailType;
        public string detailName;
        public Sprite detailIcon;
        [TextArea] public string detailDescription;
        [HideInInspector] public bool useGold;
        [HideInInspector] public Sprite cacheIcon;
        [HideInInspector] public uint basePrice;
    }
}