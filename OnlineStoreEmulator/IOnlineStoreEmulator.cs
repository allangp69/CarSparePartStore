namespace OnlineStoreEmulator;

public interface IOnlineStoreEmulator
{
    event EventHandler<IsRunningEventArgs>  IsRunningChanged;
    void CreateOrder();
    void Start();
    void Stop();
}