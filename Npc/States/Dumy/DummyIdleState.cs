using CIW.Code;
using Newtonsoft.Json.Serialization;

namespace Work.YIS.Code.Npc.States.Dumy
{
    public class DummyIdleState : AbstDummyState
    {
        public DummyIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }
    }
}