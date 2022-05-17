using System;

namespace CarSparePartStore.ViewModels.Notification;

public class NotificationAddedEventArgs
    : EventArgs
{
    public ViewModels.Notification.Notification Notification { get; }

    public NotificationAddedEventArgs(ViewModels.Notification.Notification notification)
    {
        Notification = notification;
    }
}