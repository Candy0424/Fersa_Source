using DG.Tweening;
using UnityEngine;

namespace YIS.Code.Feedbacks
{
    public class BlinkFeedback : Feedback
    {
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private float duration;

        private readonly int _blinkHash = Shader.PropertyToID("_BlinkValue"); 
        
        private Tween _tween;
        
        public override void PlayFeedback()
        {
            if (_tween.IsActive()) return;
            Material blinkMat = sr.material;
            
            
            _tween = DOVirtual.Float(1, 0, duration, 
                x => blinkMat.SetFloat(_blinkHash, x))
                .SetAutoKill(true);
        }

        public override void StopFeedback()
        {
            
        }
    }
}