using DG.Tweening;
using UnityEngine;

namespace YIS.Code.Modules
{
    public class FreezeDeBuff : Buff
    {
        private IAnimator _anim;
        private SpriteRenderer _spr;
        
        private readonly int _freezeValueHash = Shader.PropertyToID("_FreezeValue");

        private Tween _tween;
        
        protected override void AfterInit()
        {
            base.AfterInit();
            _anim = owner.GetModule<IAnimator>();
            if (_anim == null)
            {
                Debug.LogError($"애니메이터를 찾지 못함. {owner.gameObject.name}");
                return;
            }
            _spr = _anim.SpriteRenderer;
            owner.IsFreeze = true;
        }

        public override void UpdateBuffTime()
        {
            base.UpdateBuffTime();
            
        }

        protected override void ApplyBuff(float value, int duration)
        {
            _anim.StopAnimation();
            PlayEffect();
        }

        protected override void PlayEffect()
        {
            FreezeEffect(true);
            base.PlayEffect();
        }

        private void FreezeEffect(bool isFreeze)
        {
            if (_tween.IsActive()) return;
            Material freezeMat = _spr.material;

            float start = isFreeze ? -2 : 0.5f;
            float end = isFreeze ? 0.5f : -2f;
            
            _tween = DOVirtual.Float(start, end, 0.5f,
                    x => freezeMat.SetFloat(_freezeValueHash, x))
                .SetAutoKill(true);
        }

        protected override void RemoveBuff()
        {
            FreezeEffect(false);
            _anim.PlayAnimation();
            StopEffect();
            owner.IsFreeze = false;
        }
    }
}