using UnityEditor;
using UnityEngine;
using System.IO;

namespace SaveSystem.Editor
{
    public static class SaveDataTools
    {
        [MenuItem("Tools/Save Data/Delete All Saves")]
        public static void DeleteAllSaves()
        {
            string path = Application.persistentDataPath;
            int deletedCount = 0;

            foreach (var file in Directory.GetFiles(path, "save_*.dat"))
            {
                File.Delete(file);
                deletedCount++;
            }

            PlayerPrefs.DeleteKey("CurrentSaveSlot");
            PlayerPrefs.Save();

            Debug.Log($"Deleted {deletedCount} save files from {path}");
        }

        [MenuItem("Tools/Save Data/Delete Current Slot")]
        public static void DeleteCurrentSlot()
        {
            int slot = PlayerPrefs.GetInt("CurrentSaveSlot", 0);
            string filePath = Path.Combine(Application.persistentDataPath, $"save_{slot}.dat");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"Deleted save file for slot {slot} at {filePath}");
            }
            else
            {
                Debug.LogWarning($"No save file found for slot {slot}");
            }
        }
    }
}
