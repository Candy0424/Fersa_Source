using System;
using System.Collections.Generic;
using PSW.Code.BaseSystem;
using TMPro;
using UnityEngine;
using YIS.Code.Contents.QuestSystem;

namespace YIS.Code.Combat
{
    public class QuestUI : MonoBehaviour
    {
        [Header("AutoSetting")]
        [SerializeField] private QuestDataSO curQuest; // 얘는 직렬화 그냥 보기용임
        
        [Header("QuestInfo")]
        [SerializeField] private TextMeshProUGUI questName;
        [SerializeField] private TextMeshProUGUI questDesc;
        
        [Header("RewardsPrefab")]
        [SerializeField] private GameObject questRewardsPrefab;

        private List<QuestRewardUI> _questRewards;

        private void Awake()
        {
            _questRewards = new List<QuestRewardUI>();
        }

        public void Initialize(QuestDataSO quest, int questID)
        {
            curQuest = quest;
            questName.SetText(quest.quests[questID].QuestName);
            questDesc.SetText(quest.quests[questID].QuestDesc);
            
            
        }

        private void RemoveAll()
        {
            if (_questRewards.Count == 0) return;

            foreach (QuestRewardUI reward in _questRewards)
            {
                Destroy(reward.gameObject);
            }
            
            _questRewards.Clear();
        }
    }
    
}