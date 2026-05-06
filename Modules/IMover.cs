using System;
using UnityEngine;

namespace YIS.Code.Modules
{
    public interface IMover
    {
        Rigidbody2D Rigid { get; }
        event Action<Vector2> OnVelocityChange;
        void SetMoveSpeedMultiplier(float value);
        void AddForceToAgent(Vector2 force);
        void StopImmediately(bool xAxis, bool yAxis);
        void SetMovement(Vector2 movement);
    }
}