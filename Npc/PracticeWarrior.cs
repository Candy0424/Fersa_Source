using Work.CSH.Scripts.FSMSystem;

namespace Work.YIS.Code.Npc
{
    public class PracticeWarrior : NpcEntity
    {
        protected override void Start()
        {
            base.Start();
            
            ChangeState(PracticeWarriorState.IDLE);
        }

        public void ChangeState(PracticeWarriorState newState) => stateMachine.ChangeState((int)newState);
    }
}