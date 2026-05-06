using System;
using UnityEngine;

namespace YIS.Code.Modules
{
    public interface IAttackModule
    {
        ModuleOwner Owner { get; }

        event Action OnAttackEnd;
        void Attack(GameObject target = null);
    }
}