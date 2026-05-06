using CIW.Code;
using Work.CSH.Scripts.FSMSystem;

namespace Work.YIS.Code.Npc.PrWarriorState.States
{
    public class PracticeWarriorIdleState : AbstPracticeWarriorState
    {
        public PracticeWarriorIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Update()
        {
            base.Update();
            if (_triggerCall)
            {
                practiceWarrior.ChangeState(PracticeWarriorState.ATTACK);
            }
        }
    }
}