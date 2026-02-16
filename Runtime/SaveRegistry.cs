using System.Collections.Generic;

namespace SaveSystem
{
    public class SaveRegistry
    {
        private static readonly Dictionary<string, ISaveable> saveables = new();

        public static void Register(ISaveable saveable)
        {
            saveables[saveable.SaveKey] = saveable;
        }

        public static void Unregister(string key)
        {
            saveables.Remove(key);
        }

        public static Dictionary<string, object> CaptureAll()
        {
            var data = new Dictionary<string, object>();
            foreach (var kvp in saveables)
            {
                data[kvp.Key] = kvp.Value.CaptureState();
            }
            return data;
        }

        public static object GetSavedState(string key, Dictionary<string, object> fullSave)
        {
            return fullSave.TryGetValue(key, out var state) ? state : null;
        }

    }
}
