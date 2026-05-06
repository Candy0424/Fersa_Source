using PSB_Lib.ObjectPool.RunTime;
using UnityEngine;
using YIS.Code.Defines;

namespace YIS.Code.Modules
{
    [CreateAssetMenu(fileName = "New BuffData", menuName = "SO/Buff/BuffDataSO", order = 0)]
    public class BuffDataSO : ScriptableObject
    {
        public string buffEnumName;
        public int index;
        public Elemental elemental;
        public BuffVisualSO visualData;
        public PoolItemSO buffPrefab;
    }
}