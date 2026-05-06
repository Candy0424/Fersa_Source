using System.Collections.Generic;
using CIW.Code;
using UnityEngine;
using YIS.Code.Skills.Interfaces;
using YIS.Code.Skills.Sequences;

namespace YIS.Code.Skills
{
    public class TestFreezeSkill : BaseSkill, IAttackSkill
    {
        protected override IReadOnlyList<ISkillAction> NormalSkillGenerateAction(Entity user, IReadOnlyList<Entity> targets)
        {
            return null;
        }

        protected override IReadOnlyList<ISkillAction> ChainSkillGenerateAction(Entity user, IReadOnlyList<Entity> targets)
        {
            return null;
        }

        public void UseAttackSkill()
        {
            
        }
    }
}