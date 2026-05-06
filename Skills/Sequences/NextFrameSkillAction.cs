using System.Threading.Tasks;
using UnityEngine;

namespace YIS.Code.Skills.Sequences
{
    public class NextFrameSkillAction : ISkillAction
    {
        public async Task ExecuteAsync()
        {
            await Awaitable.NextFrameAsync();
        }
    }
}