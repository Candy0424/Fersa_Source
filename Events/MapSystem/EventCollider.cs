using System;
using UnityEngine;

namespace YIS.Code.Events.MapSystem
{
    public class EventCollider : MonoBehaviour
    {
        public event Action OnHit;
        public event Action OnEnd;
        
        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnHit?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            OnEnd?.Invoke();
        }
    }
}