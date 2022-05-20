namespace CarSparePartStore.ViewModels.Notification.EventArgs;

public class NotificationRemovedEventArgs
    : System.EventArgs
{
    public Notification Notification { get; }

    public NotificationRemovedEventArgs(Notification notification)
    {
        Notification = notification;
    }
}