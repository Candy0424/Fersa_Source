using UnityEngine;

namespace YIS.Code.CoreSystem
{
    public abstract class BaseCommandSO : ScriptableObject, ICommandable
    {
        public abstract bool CanHandle(Context context);
        public abstract bool Handle(Context context);
    }
}