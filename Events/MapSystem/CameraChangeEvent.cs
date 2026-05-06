using System;
using System.Collections.Generic;
using System.Linq;
using PSW.Code.EventBus;
using Unity.Cinemachine;
using UnityEngine;

namespace YIS.Code.Events.MapSystem
{
    public class CameraChangeEvent : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera targetCam;

        private List<EventCollider> _colliders;
        
        private void Awake()
        {
            _colliders = GetComponentsInChildren<EventCollider>().ToList();

            RegistryEvent();
        }

        private void RegistryEvent()
        {
            foreach (var col in _colliders)
            {
                col.OnHit += HandleHit;
                col.OnEnd += HandleEnd;
            }
        }

        private void OnDestroy()
        {
            foreach (var col in _colliders)
            {
                col.OnHit -= HandleHit;
                col.OnEnd -= HandleEnd;
            }
        }

        private void HandleHit()
        {
            targetCam.Priority = 100;
            Bus<HallEnterUIEvent>.Raise(new HallEnterUIEvent(true));
        }
        
        private void HandleEnd()
        {
            targetCam.Priority = -100;
            Bus<HallEnterUIEvent>.Raise(new HallEnterUIEvent(false));
        }
    }
}