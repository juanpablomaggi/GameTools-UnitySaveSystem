namespace SaveSystem
{
    public interface ISaveable
    {
        string SaveKey { get; } // IMPORTANT: Must be unique for each ISaveable object
        object CaptureState();
        void RestoreState(object state);
    }
}
