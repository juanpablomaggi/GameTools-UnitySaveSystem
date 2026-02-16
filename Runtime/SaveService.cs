using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveSystem
{
    public class SaveService : ISaveService
    {
        private static string CURRENT_SAVE_SLOT_PREF = "CurrentSaveSlot";
        private int currentSaveSlot;
        private SaveRegistry saveRegistry;
        private Dictionary<string, object> loadedData;

        public int CurrentSaveSlot => currentSaveSlot;

        public void Init()
        {
            saveRegistry = new SaveRegistry();
            loadedData = new Dictionary<string, object>();
            currentSaveSlot = PlayerPrefs.GetInt(CURRENT_SAVE_SLOT_PREF, 0);
        }

        public void RegisterSaveable<T>(ISaveable<T> saveable)
        {
            saveRegistry.Register(saveable);

            if (loadedData.TryGetValue(saveable.SaveKey, out var state))
            {
                saveable.RestoreState((T)state);
            }
        }

        public void UnregisterSaveable(string key) => saveRegistry.Unregister(key);

        public void SaveGame()
        {
            SaveGame(currentSaveSlot);
        }
        public void SaveGame(int slotNumber)
        {
            if (currentSaveSlot != slotNumber)
            {
                PlayerPrefs.SetInt(CURRENT_SAVE_SLOT_PREF, slotNumber);
                currentSaveSlot = slotNumber;
            }

            var saveData = new SaveData { data = saveRegistry.CaptureAll() };
            string path = GetPath(currentSaveSlot);

            using FileStream stream = File.Create(path);
            new BinaryFormatter().Serialize(stream, saveData);
        }

        public bool LoadGame()
        {
            return LoadGame(currentSaveSlot);
        }
        public bool LoadGame(int slotNumber)
        {
            string path = GetPath(slotNumber);
            if (!File.Exists(path)) return false;

            if (currentSaveSlot != slotNumber)
            {
                PlayerPrefs.SetInt(CURRENT_SAVE_SLOT_PREF, slotNumber);
                currentSaveSlot = slotNumber;
            }

            using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var saveData = (SaveData)new BinaryFormatter().Deserialize(stream);

            loadedData = saveData.data;
            saveRegistry.RestoreAll(loadedData);
            return true;
        }

        public void DeleteSave()
        {
            DeleteSave(currentSaveSlot);
        }
        public void DeleteSave(int slotNumber)
        {
            string filePath = Path.Combine(Application.persistentDataPath, $"save_{slotNumber}.dat");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"Deleted save file for slot {slotNumber} at {filePath}");
            }
            else
            {
                Debug.LogWarning($"No save file found for slot {slotNumber}");
            }
        }

        private string GetPath(int slotNumber)
        {
            return Path.Combine(Application.persistentDataPath, $"save_{slotNumber}.dat");
        }
    }
}
