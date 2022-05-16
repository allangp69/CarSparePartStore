using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace CarSparePartStore.ViewModels;

public class NotificationHandler
    : IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private static object _listLockObject = new object();
    private readonly IConfiguration _configuration;
    private int _defaultPeriodSeconds = 5;
    public event EventHandler<NotificationAddedEventArgs> NotificationAdded;
    public event EventHandler<NotificationRemovedEventArgs> NotificationRemoved;

    public NotificationHandler()
    {
        _configuration = Ioc.Default.GetRequiredService<IConfiguration>();
        Notifications = new List<Notification>();
        _cancellationTokenSource = new CancellationTokenSource();
        var thread = new Thread(() =>
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var deleteList = new List<Notification>();
                lock (_listLockObject)
                {
                    deleteList.AddRange(Notifications.Where(notification => notification.Created.Add(ShowNotificationInterval) < DateTime.Now));
                    Notifications.ForEach(n => n.IsActive = !deleteList.Contains(n));
                    Notifications.RemoveAll(n => deleteList.Contains(n));
                    OnNotificationsRemoved(deleteList);
                }
                _cancellationTokenSource.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(1));   
            }
        });
        thread.Start();
    }

    private void OnNotificationsRemoved(IEnumerable<Notification> removedNotifications)
    {
        foreach (var notification in removedNotifications)
        {
            OnNotificationRemoved(notification);
        }
    }

    private TimeSpan ShowNotificationInterval => TimeSpan.FromSeconds(int.TryParse(_configuration.GetSection("ApplicationSettings").GetSection("ShowNotificationsSeconds").Value, out var period) ? period : _defaultPeriodSeconds);

    public void AddNotification(Notification notification)
    {
        lock (_listLockObject)
        {
            Notifications.Add(notification);
        }
        OnNotificationAdded(notification);
    }

    private void OnNotificationAdded(Notification notification)
    {
        var handler = NotificationAdded;
        handler?.Invoke(this,  new NotificationAddedEventArgs(notification));
    }
    
    private void OnNotificationRemoved(Notification notification)
    {
        var handler = NotificationRemoved;
        handler?.Invoke(this,  new NotificationRemovedEventArgs(notification));
    }
    
    private List<Notification> Notifications { get; }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }

    public List<Notification> GetActiveNotifications()
    {
        return Notifications.Where(n => n.IsActive).ToList();
    }
}