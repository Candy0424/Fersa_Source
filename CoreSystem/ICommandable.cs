using CIW.Code;
using UnityEngine;
using Work.YIS.Code.Skills;

namespace YIS.Code.CoreSystem
{
    public struct Context
    {
        public bool CanChain;
        public SkillEnum[] SkillIds;
        public bool[] ChainFlags;

        public Entity Caster;
        public Transform Target;
        public ISkillExecutor Executor;

        public Context(bool canChain, SkillEnum[] skillIds, Entity caster, Transform target, ISkillExecutor executor, bool[] chainFlags = null)
        {
            CanChain = canChain;
            SkillIds = skillIds;
            Caster = caster;
            Target = target;
            Executor = executor;
            ChainFlags = chainFlags;
        }
    }

    public interface ISkillExecutor
    {
        bool CanExecuteById(SkillEnum id, Transform target);
        bool ExecuteById(SkillEnum id, bool isChain, Transform target);
    }

    public interface ICommandable
    {
        bool CanHandle(Context context);
        bool Handle(Context context);
    }
    
}