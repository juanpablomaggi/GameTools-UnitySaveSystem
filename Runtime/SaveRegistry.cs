using System.Collections.Generic;

namespace SaveSystem
{
    internal class SaveRegistry
    {
        private static readonly Dictionary<string, ISaveableWrapper> saveables = new();

        internal void Register<T>(ISaveable<T> saveable)
        {
            saveables[saveable.SaveKey] = new SaveableWrapper<T>(saveable);
        }

        internal void Unregister(string key)
        {
            saveables.Remove(key);
        }

        internal Dictionary<string, object> CaptureAll()
        {
            var data = new Dictionary<string, object>();
            foreach (var kvp in saveables)
            {
                data[kvp.Key] = kvp.Value.CaptureStateAsObject();
            }
            return data;
        }

        internal void RestoreAll(Dictionary<string, object> fullSave)
        {
            foreach (var kvp in saveables)
            {
                if (fullSave.TryGetValue(kvp.Key, out var state))
                {
                    kvp.Value.RestoreStateFromObject(state);
                }
            }
        }
    }

    internal interface ISaveableWrapper
    {
        string SaveKey { get; }
        object CaptureStateAsObject();
        void RestoreStateFromObject(object state);
    }

    internal class SaveableWrapper<T> : ISaveableWrapper
    {
        private readonly ISaveable<T> inner;

        public SaveableWrapper(ISaveable<T> inner)
        {
            this.inner = inner;
        }

        public string SaveKey => inner.SaveKey;
        public object CaptureStateAsObject() => inner.CaptureState();
        public void RestoreStateFromObject(object state) => inner.RestoreState((T)state);
    }

}
