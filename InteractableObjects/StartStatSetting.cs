using PSW.Code.EventBus;
using UnityEngine;
using Work.CSH.Scripts.Interacts;
using YIS.Code.Events;

namespace YIS.Code.InteractableObjects
{
    public class StartStatSetting : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public string Name { get; set; }
        public Transform Transform => transform;
        public void OnInteract()
        {
            Bus<StartStatSettingUIActiveEvent>.Raise(new StartStatSettingUIActiveEvent(true));
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            Bus<StartStatSettingUIActiveEvent>.Raise(new StartStatSettingUIActiveEvent(false));
        }
    }
}
