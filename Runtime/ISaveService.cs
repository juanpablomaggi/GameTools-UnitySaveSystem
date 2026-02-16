using System.Collections.Generic;

namespace SaveSystem
{
    public interface ISaveService
    {
        void Init();
        int CurrentSaveSlot { get; }

        void RegisterSaveable<T>(ISaveable<T> saveable);
        void UnregisterSaveable(string saveableKey);

        void SaveGame();
        void SaveGame(int slotNumber);

        bool LoadGame();
        bool LoadGame(int slotNumber);

        void DeleteSave();
        void DeleteSave(int slotNumber);
    }
}

