using System;
using UnityEngine;
using YIS.Code.Skills;

namespace YIS.Code.Contents.QuestSystem
{
    [Serializable]
    public struct QuestInfo
    {
        public string QuestName;
        [TextArea] public string QuestDesc;
        public SkillDataSO[] rewardSkill;
    }
    
    [CreateAssetMenu(fileName = "NewQuestData", menuName = "SO/Content/QuestData", order = 0)]
    public class QuestDataSO : ScriptableObject
    {
        public string mainQuestName;
        public QuestInfo[] quests;
    }
}