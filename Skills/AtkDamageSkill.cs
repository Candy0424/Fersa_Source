using System.Collections.Generic;
using CIW.Code;
using UnityEngine;
using Work.YIS.Code.Buffs;
using YIS.Code.Combat;
using YIS.Code.Modules;
using YIS.Code.Skills.Interfaces;
using YIS.Code.Skills.Sequences;

namespace YIS.Code.Skills
{
    public class AtkDamageSkill : BaseSkill, IBuffOrDeBuffSkill
    {
        BuffModule _playerBuff;
        bool _buffCall = false;

        public override float GetFinalDamage()
        {
            float baseDmg = SkillData.damage;

            if (_buffCall)
            {
                return 0f;
            }

            return baseDmg;
        }

        protected override IReadOnlyList<ISkillAction> ChainSkillGenerateAction(Entity user, IReadOnlyList<Entity> targets)
        {
            List<ISkillAction> actions = new List<ISkillAction>();
            
            actions.Add(new PlayEffectCallbackAction(this, targets));
            actions.Add(new BuffSkillAction(user, BuffType.ATTACK_BUFF, SkillData.damage * 2, SkillData.durationTurn));
            
            _buffCall = true;
            SkipDamageThisCast = true;
            
            Debug.Log($"공격력 증가 스킬 체인 사용! 데미지 버프 적용 : {SkillData.damage}");

            return actions;
        }

        protected override IReadOnlyList<ISkillAction> NormalSkillGenerateAction(Entity user, IReadOnlyList<Entity> targets)
        {
            List<ISkillAction> actions = new List<ISkillAction>();
            
            DamageData damageData = new DamageData(SkillData.damage, CurrentElementalState.CurrentElemental);
            
            actions.Add(new PlayEffectCallbackAction(this, targets));
            actions.Add(new DamageSkillAction(user, targets, damageData, SkillData.impulsePower));
            
            Debug.Log($"단독 실행. 버프는 없고 데미지만 줄게");
            return actions;
        }
        
        public override bool SkipAnim(bool isChain)
        {
            return isChain;
        }

        public void UseBuffOrDeBuffSkill()
        {
        }
    }
}