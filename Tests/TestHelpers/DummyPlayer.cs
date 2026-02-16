namespace SaveSystem.Test
{
    public class DummyPlayer : ISaveable<DummySaveData>
    {
        public string SaveKey => "DummyPlayer";
        public string Name { get; set; }
        public int Health { get; set; }

        public DummySaveData CaptureState()
        {
            return new DummySaveData { Name = Name, Health = Health };
        }

        public void RestoreState(DummySaveData state)
        {
            Name = state.Name;
            Health = state.Health;
        }
    }

    [System.Serializable]
    public class DummySaveData
    {
        public int Health { get; set; } = 100;
        public string Name { get; set; } = "TestPlayer";
    }
}