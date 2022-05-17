using System;

namespace CarSparePartStore.ViewModels.Notification;

public class NotificationRemovedEventArgs
    : EventArgs
{
    public ViewModels.Notification.Notification Notification { get; }

    public NotificationRemovedEventArgs(ViewModels.Notification.Notification notification)
    {
        Notification = notification;
    }
}