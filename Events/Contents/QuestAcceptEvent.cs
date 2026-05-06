using PSW.Code.EventBus;
using YIS.Code.Contents.QuestSystem;

namespace YIS.Code.Events.Contents
{
    public struct QuestAcceptEvent : IEvent
    {
        public QuestDataSO Quest;

        public QuestAcceptEvent(QuestDataSO quest)
        {
            Quest = quest;
        }
    }
}