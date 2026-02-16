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
        public void Init()
        {
            currentSaveSlot = PlayerPrefs.GetInt(CURRENT_SAVE_SLOT_PREF, 0);
        }

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

            var saveData = new SaveData { data = SaveRegistry.CaptureAll() };
            string path = GetPath(currentSaveSlot);

            using FileStream stream = File.Create(path);
            new BinaryFormatter().Serialize(stream, saveData);
        }

        public Dictionary<string, object> LoadGame()
        {
            return LoadGame(currentSaveSlot);
        }

        public Dictionary<string, object> LoadGame(int slotNumber)
        {
            string path = GetPath(slotNumber);
            if (!File.Exists(path)) return null;

            if (currentSaveSlot != slotNumber)
            {
                PlayerPrefs.SetInt(CURRENT_SAVE_SLOT_PREF, slotNumber);
                currentSaveSlot = slotNumber;
            }

            using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var saveData = (SaveData)new BinaryFormatter().Deserialize(stream);
            return saveData.data;
        }

        private string GetPath(int slotNumber)
        {
            return Path.Combine(Application.persistentDataPath, $"save_{slotNumber}.dat");
        }
    }
}
