using System;
using PSW.Code.EventBus;
using UnityEngine;
using Work.CSH.Scripts.Interacts;
using YIS.Code.Events.Contents;

namespace YIS.Code.Contents.QuestSystem
{
    public class QuestGiver : MonoBehaviour, IInteractable
    {
        [SerializeField] private QuestDataSO questData;

        [field: SerializeField] public string Name { get; set; }
        public Transform Transform => gameObject.transform;
        
        
        public void OnInteract()
        {
            QuestGive();
        }
        
        private void QuestGive()
        {
            Bus<QuestAcceptEvent>.Raise(new QuestAcceptEvent(questData));
        }
    }
}