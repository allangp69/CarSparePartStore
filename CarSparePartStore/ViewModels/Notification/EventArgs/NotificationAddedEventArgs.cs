namespace CarSparePartStore.ViewModels.Notification.EventArgs;

public class NotificationAddedEventArgs
    : System.EventArgs
{
    public Notification Notification { get; }

    public NotificationAddedEventArgs(Notification notification)
    {
        Notification = notification;
    }
}