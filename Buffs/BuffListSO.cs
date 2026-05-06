using UnityEngine;

namespace YIS.Code.Modules
{
    [CreateAssetMenu(fileName = "New BuffListSO", menuName = "SO/Buff/BuffListSO", order = 0)]
    public class BuffListSO : ScriptableObject
    {
        public string enumName;
        public BuffDataSO[] buffs;
    }
}