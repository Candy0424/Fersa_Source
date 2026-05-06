using System;
using UnityEngine;

namespace YIS.Code.Effects
{
    public class EffectTrigger : MonoBehaviour
    {
        public event Action OnEndTrigger;
        
        public void EndTrigger() => OnEndTrigger?.Invoke();

        public void ResetEvent()
        {
            OnEndTrigger = null;
        }
    }
}