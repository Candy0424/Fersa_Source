using System.Threading.Tasks;
using CIW.Code;
using Work.YIS.Code.Buffs;
using YIS.Code.Modules;

namespace YIS.Code.Skills.Sequences
{
    public class BuffSkillAction : ISkillAction
    {
        private readonly Entity _target;
        public BuffType BuffType { get; private set; }
        public float Value { get; private set; }
        public int Duration { get; private set; }

        public BuffSkillAction(Entity target, BuffType buffType, float value, int duration)
        {
            _target = target;
            BuffType = buffType;
            Value = value;
            Duration = duration;
        }
        
        public async Task ExecuteAsync()
        {
            if (_target == null) return;

            BuffModule buff = _target.GetModule<BuffModule>();
            if (buff == null) return;
            
            buff.BuffApply(BuffType, Value, Duration);
        }
    }
}