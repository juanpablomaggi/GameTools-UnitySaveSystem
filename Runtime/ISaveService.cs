using System.Collections.Generic;

namespace SaveSystem
{
    public interface ISaveService
    {
        void Init();
        void SaveGame();
        void SaveGame(int slotNumber);
        Dictionary<string, object> LoadGame();
        Dictionary<string, object> LoadGame(int slotNumber);
    }
}

