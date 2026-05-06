using PSW.Code.EventBus;
using UnityEngine;
using YIS.Code.Defines;

namespace YIS.Code.Events
{
    public struct DmgTextUiEvent : IEvent
    {
        public Vector2 TargetPos;
        public float Value;
        public Elemental ElementalType;

        public DmgTextUiEvent(Vector2 targetPos, float value, Elemental elementalType)
        {
            TargetPos = targetPos;
            Value = value;
            ElementalType = elementalType;
        }
    }
}