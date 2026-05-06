using System.Collections.Generic;
using PSW.Code.EventBus;
using UnityEngine;
using YIS.Code.Events.Contents;

namespace YIS.Code.Contents.QuestSystem
{
    public class QuestContent : MonoBehaviour
    {
        private Dictionary<QuestDataSO, int> _quests;

        private void Awake()
        {
            _quests = new Dictionary<QuestDataSO, int>();

            RegistryEvent();
        }

        private void RegistryEvent()
        {
            Bus<QuestAcceptEvent>.OnEvent += HandleQuestAccept;
        }

        private void OnDestroy()
        {
            Bus<QuestAcceptEvent>.OnEvent -= HandleQuestAccept;
        }

        private void HandleQuestAccept(QuestAcceptEvent evt)
        {
            AddQuest(evt.Quest);
        }

        private void AddQuest(QuestDataSO quest)
        {
            _quests.Add(quest, 0);
        }

        private void RemoveQuest(QuestDataSO quest)
        {
            _quests.Remove(quest);
        }

        private bool SuccessQuest(QuestDataSO quest)
        {
            bool isFind = _quests.ContainsKey(quest);
            if (!isFind)
                return false;

            _quests[quest]++;
            
            return true;
        }

        public Dictionary<QuestDataSO, int> GetQuests() => _quests;
    }
}