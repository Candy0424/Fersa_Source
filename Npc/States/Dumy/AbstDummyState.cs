using CIW.Code;
using CIW.Code.FSM;

namespace Work.YIS.Code.Npc.States.Dumy
{
    public class AbstDummyState : EntityState
    {
        protected Dummy dummy;
        
        public AbstDummyState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            dummy = entity as Dummy;
        }
    }
}