namespace OnlineStoreEmulator;

public class IsRunningEventArgs
    :EventArgs
{
    public IsRunningEventArgs(bool isRunning)
    {
        IsRunning = isRunning;
    }

    public bool IsRunning { get; set; }
}