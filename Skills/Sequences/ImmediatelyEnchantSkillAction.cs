using System.Threading.Tasks;
using CIW.Code;
using UnityEngine;
using YIS.Code.Defines;
using YIS.Code.Modules;

namespace YIS.Code.Skills.Sequences
{
    public class ImmediatelyEnchantSkillAction : ISkillAction
    {
        private Entity _user;
        private BaseSkill _skill;
        private Elemental _elemental;

        public ImmediatelyEnchantSkillAction(Entity entity, BaseSkill skill, Elemental elemental)
        {
            _user = entity;
            _skill = skill;
            _elemental = elemental;
        }
        
        public async Task ExecuteAsync()
        {
            BuffModule buffModule = _user.GetModule<BuffModule>();
            
            Debug.Assert(buffModule != null, $"버프 모듈이 없습니다. {_user}");

            buffModule.ImmediatelyBuff(_elemental);
        }
    }
}