using System;

namespace CarSparePartStore.ViewModels;

public class NotificationAddedEventArgs
    : EventArgs
{
    public Notification Notification { get; }

    public NotificationAddedEventArgs(Notification notification)
    {
        Notification = notification;
    }
}