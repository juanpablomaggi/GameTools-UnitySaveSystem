namespace SaveSystem
{
    public interface ISaveable<T>
    {
        string SaveKey { get; } // IMPORTANT: Must be unique for each ISaveable object
        T CaptureState();
        void RestoreState(T state);
    }
}