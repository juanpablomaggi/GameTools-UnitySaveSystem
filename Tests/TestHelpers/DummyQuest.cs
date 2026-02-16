namespace SaveSystem.Test
{
    public class DummyQuest : ISaveable<DummyQuestState>
    {
        public string SaveKey => "DummyQuest";
        public string QuestName { get; set; }
        public bool IsCompleted { get; set; }

        public DummyQuestState CaptureState()
        {
            return new DummyQuestState { questName = QuestName, isCompleted = IsCompleted };
        }

        public void RestoreState(DummyQuestState state)
        {
            QuestName = state.questName;
            IsCompleted = state.isCompleted;
        }
    }

    [System.Serializable]
    public class DummyQuestState
    {
        public string questName;
        public bool isCompleted;
    }
}