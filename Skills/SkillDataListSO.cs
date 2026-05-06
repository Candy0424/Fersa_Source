using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Work.YIS.Code.Skills;

namespace YIS.Code.Skills
{
    [CreateAssetMenu(fileName = "NewSkillList", menuName = "SO/Skill/SkillDataList", order = 0)]
    public class SkillDataListSO : ScriptableObject
    {
        public string enumName;
        public SkillDataSO[] skills;

        private Dictionary<int, SkillDataSO> skillDict;

        private void OnEnable()
        {
            Rebuild();
        }

        private void Rebuild()
        {
            if (skills == null) skills = new SkillDataSO[0];
            skillDict = skills.Where(s => s != null).ToDictionary(s => s.index, s => s);
        }

        public SkillDataSO FindSkill(int index)
        {
            if (skillDict == null) Rebuild();
            return skillDict.TryGetValue(index, out var so) ? so : null;
        }

        public SkillDataSO FindSkill(SkillEnum id) => FindSkill((int)id);
        
    }
}