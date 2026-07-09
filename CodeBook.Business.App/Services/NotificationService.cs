using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Interfaces;
using AutoMapper;
using CodeBook.Data.App.IRepositories;
using CodeBook.Models.App;
using System;
using CodeBook.Business.App.Middleware;


namespace CodeBook.Business.App.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper mapper;
        public NotificationService(INotificationRepository notificationRepository, IMapper mapper)
        {
            this._notificationRepository = notificationRepository;
            this.mapper = mapper;
        }
        public void CreateNotification(int userId, NotificationDTO notificationDto)
        {
            var notification = new Notification
            {
                UserId = userId,
                Type = Enum.Parse<NotificationType>(notificationDto.Type),
                ReferenceId = notificationDto.ReferenceId,
                Message = notificationDto.Message,
                IsSeen = false,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                SenderId = notificationDto.SenderId
            };
            _notificationRepository.Add(notification);
            _notificationRepository.SaveChanges();
        }
        public List<NotificationDTO> GetUserNotification(int userId)
        {
            var notifications = _notificationRepository.GetNotificationsbyUserId(userId).OrderByDescending(n => n.DateCreated);
            return mapper.Map<List<NotificationDTO>>(notifications);
        }
        public void MarkAsRead(int notificationId)
        {
            var notification = _notificationRepository.GetbyNotificationId(notificationId);

            if (notification != null)
            {
                notification.IsSeen = true;
                notification.DateUpdated = DateTime.UtcNow;
                _notificationRepository.Update(notification);
                _notificationRepository.SaveChanges();
            }
        }
        public int GetUnreadNotificationCount(int userId)
        {
            var notifications = _notificationRepository.GetNotificationsbyUserId(userId);
            return notifications.Count(n => n.IsSeen == false);

        }
        public void MarkAllAsRead(int userId)
        {
            List<Notification> notifications = _notificationRepository.GetNotificationsbyUserId(userId);

            if (notifications != null)
            {
                foreach (Notification notification in notifications)
                {
                    MarkAsRead(notification.Id);
                }
            }
        }

    }
}
