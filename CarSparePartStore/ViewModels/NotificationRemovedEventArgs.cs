using System;

namespace CarSparePartStore.ViewModels;

public class NotificationRemovedEventArgs
    : EventArgs
{
    public Notification Notification { get; }

    public NotificationRemovedEventArgs(Notification notification)
    {
        Notification = notification;
    }
}