using PSW.Code.EventBus;
using UnityEngine;

namespace YIS.Code.Events
{
    public class ImpulseEvent : IEvent
    {
        public Vector3 impulseVelocity;

        public ImpulseEvent(Vector3 impulseVelocity)
        {
            this.impulseVelocity = impulseVelocity;
        }
    }
}