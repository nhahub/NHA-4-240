using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Middleware;
using CodeBook.Models.App;
using System;
namespace CodeBook.Business.App.Interfaces
{
    public interface INotificationService
    {
        public void CreateNotification(int userId, NotificationDTO notificationDto);
        List<NotificationDTO> GetUserNotification(int userId);
        void MarkAsRead(int notificationId);
        int GetUnreadNotificationCount(int userId);
        void MarkAllAsRead(int userId);
    }
}
