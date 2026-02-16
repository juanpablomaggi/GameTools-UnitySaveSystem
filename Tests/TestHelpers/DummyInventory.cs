using System.Collections.Generic;

namespace SaveSystem.Test
{
    public class DummyInventory : ISaveable<DummyInventoryState>
    {
        public string SaveKey => "DummyInventory";
        public List<string> Items { get; set; } = new();

        public DummyInventoryState CaptureState()
        {
            return new DummyInventoryState { items = new List<string>(Items) };
        }

        public void RestoreState(DummyInventoryState state)
        {
            Items = new List<string>(state.items);
        }
    }

    [System.Serializable]
    public class DummyInventoryState
    {
        public List<string> items = new();
    }
}
