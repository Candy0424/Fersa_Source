using System.Collections.Generic;
using PSW.Code.EventBus;
using YIS.Code.Contents.QuestSystem;

namespace YIS.Code.Events.Contents
{
    public struct QuestUpdateEvent : IEvent
    {
        public readonly Dictionary<QuestDataSO, int> Quests;
        
        public QuestUpdateEvent(Dictionary<QuestDataSO, int> quests)
        {
            Quests = quests;
        }
    }
}