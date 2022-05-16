using System;

namespace CarSparePartStore.ViewModels;

public class Notification
{
    public Notification(string notification)
    {
        Message = notification;
        Created = DateTime.Now;
        IsActive = true;
        Guid = Guid.NewGuid();
    }
    public Guid Guid { get; set; }
    public string Message { get; }
    public DateTime Created { get; }
    public bool IsActive { get; set; }
}