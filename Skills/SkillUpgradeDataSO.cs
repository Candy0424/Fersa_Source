using UnityEngine;

namespace YIS.Code.Skills
{
    [CreateAssetMenu(fileName = "New Skill UpgradeData", menuName = "SO/Skill/SkillUpgradeData", order = 0)]
    public class SkillUpgradeDataSO : ScriptableObject
    {
        public float damageBonus; // 공격력 증가량
        public int turnBonus; // 턴 감소량
        public int buffDurationBonus; // 버프 지속턴 증가량
        public float buffPowerBonus; // 버프량 증가량
        public int rangeBonus; // 범위 증가량
    }
}