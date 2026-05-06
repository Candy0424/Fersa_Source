using CIW.Code;
using CIW.Code.FSM;
using Work.YIS.Code.Npc;

namespace Work.YIS.Code.Npc.PrWarriorState.States
{
    public class AbstPracticeWarriorState : EntityState
    {
        protected PracticeWarrior practiceWarrior;
        
        public AbstPracticeWarriorState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            practiceWarrior = entity as PracticeWarrior;
        }
    }
}