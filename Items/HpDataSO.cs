using UnityEngine;

namespace YIS.Code.Items
{
    [CreateAssetMenu(fileName = "New HpData", menuName = "SO/Item/HpData", order = 0)]
    public class HpDataSO : ScriptableObject
    {
        public float value;
        
        public ItemVisualDataSO visualData;
    }
}