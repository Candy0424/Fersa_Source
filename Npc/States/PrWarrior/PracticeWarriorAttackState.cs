using CIW.Code;
using UnityEngine;
using Work.CSH.Scripts.FSMSystem;
using Work.YIS.Code.Npc.Components;

namespace Work.YIS.Code.Npc.PrWarriorState.States
{
    public class PracticeWarriorAttackState : AbstPracticeWarriorState
    {
        private NpcNormalAttackComponent _attackCompo;
        
        public PracticeWarriorAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _attackCompo = practiceWarrior.GetModule<NpcNormalAttackComponent>();
            
            _animatorTrigger.OnAttackTrigger += HandleAttack;
        }

        private void HandleAttack()
        {
            _attackCompo.Attack();
        }

        public override void Update()
        {
            base.Update();
            
            if (_triggerCall)
                practiceWarrior.ChangeState(PracticeWarriorState.IDLE);
        }

        public override void Exit()
        {
            _animatorTrigger.OnAttackTrigger -= HandleAttack;
            
            base.Exit();
        }
    }
}