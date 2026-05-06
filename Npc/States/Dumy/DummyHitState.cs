using CIW.Code;
using UnityEngine;
using Work.CSH.Scripts.FSMSystem;

namespace Work.YIS.Code.Npc.States.Dumy
{
    public class DummyHitState : AbstDummyState
    {
        public DummyHitState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }
        
        public override void Update()
        {
            base.Update();

            if (_triggerCall)
                dummy.ChangeState(DummyState.IDLE);
        }
    }
}