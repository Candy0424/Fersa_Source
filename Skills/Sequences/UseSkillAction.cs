using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CIW.Code;
using UnityEngine;
using UnityEngine.Rendering;

namespace YIS.Code.Skills.Sequences
{
    public class UseSkillAction : IBattleAction
    {
        private readonly Entity _user;
        private BaseSkill _skill;
        private bool _isChain;
        private readonly IReadOnlyList<Entity> _targets;

        public UseSkillAction(Entity entity, BaseSkill skill, bool isChain, IReadOnlyList<Entity> targets)
        {
            _user = entity;
            _skill = skill;
            _isChain = isChain;
            _targets = targets;
        }

        ~UseSkillAction()
        {
            Debug.Log("스킬 액션 소멸");
        }
        public async Task ExecuteAsync()
        {
            if (_user == null || _user.IsDead || _skill == null || _targets.Count <= 0)
                //if (_user == null || _user.IsDead || _skill == null)
                return;

            IReadOnlyList<ISkillAction> actions = _skill.GenerateSkill(_isChain, _user, _targets);

            if (actions == null) return;
            
            Debug.Log($"스킬 명령 개수1 {actions.Count}");
            int index = -1;
            try
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    index = i;
                    Debug.Log($"해당 스킬 {actions[i]}");
                    if (actions[i] == null)
                    {
                        Debug.LogWarning($"인덱스 널 : {i}");
                    }
                    await actions[i].ExecuteAsync();
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"ID: {index}");
                Debug.LogError(e);
                throw;
            }
            
            
            return;
        }
    }
}