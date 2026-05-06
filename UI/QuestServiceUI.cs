using System.Collections.Generic;
using PSW.Code.BaseSystem;
using PSW.Code.EventBus;
using UnityEngine;
using YIS.Code.Contents.QuestSystem;
using YIS.Code.Events.Contents;

namespace YIS.Code.Combat
{
    public class QuestServiceUI : MonoBehaviour, IBaseSystemUI
    {
        [SerializeField] private GameObject questPrefab;
        [SerializeField] private Transform root;

        private Dictionary<QuestDataSO, int> _questsLookup;

        private List<QuestUI> _quests;

        private void Awake()
        {
            _quests = new List<QuestUI>();
            
            Bus<QuestUpdateEvent>.OnEvent += HandleQuestUpdate;
        }

        public void DataInit()
        {
            RemoveAll();
            
            // foreach ((QuestDataSO quest, int stage) in _questsLookup)
            // {
            //     GameObject questGo = Instantiate(questPrefab, root);
            //     QuestUI questUI = questGo.GetComponent<QuestUI>();
            //     questUI.Initialize(quest, stage);
            //     questGo.SetActive(true);
            //     _quests.Add(questUI);
            // }
        }

        public void DataDestroy()
        {
            RemoveAll();
        }

        private void RemoveAll()
        {
            if (_quests.Count == 0) return;
            
            foreach (var t in _quests)
                Destroy(t.gameObject);

            _quests.Clear();
        }

        private void HandleQuestUpdate(QuestUpdateEvent evt) => _questsLookup = evt.Quests;
    }
}