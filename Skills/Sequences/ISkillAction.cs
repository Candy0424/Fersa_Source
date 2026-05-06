using System.Threading.Tasks;
using UnityEngine;

namespace YIS.Code.Skills.Sequences
{
    public interface ISkillAction
    {
        Task ExecuteAsync();
    }
}