using UnityEngine;

namespace YIS.Code.Modules
{
    public interface IAnimator
    {
        Animator Animator { get; }
        SpriteRenderer SpriteRenderer { get; }

        void StopAnimation();
        void PlayAnimation();
    }
}