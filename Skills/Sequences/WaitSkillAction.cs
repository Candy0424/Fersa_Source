using System.Threading.Tasks;
using UnityEngine;

namespace YIS.Code.Skills.Sequences
{
    public class WaitSkillAction : ISkillAction
    {
        private readonly float _seconds;

        public WaitSkillAction(float seconds)
        {
            _seconds = seconds;
        }
        
        public async Task ExecuteAsync()
        {
            await Awaitable.WaitForSecondsAsync(_seconds);
        }
    }
}