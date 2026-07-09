using CodeBook.Data.App.IRepositories;
using CodeBook.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBook.Data.App.IRepositories;

namespace CodeBook.Data.App.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly CodeBookContext _context;
        public NotificationRepository(CodeBookContext context) { _context = context; }
        public List<Notification> GetNotificationsbyUserId(int userId)
        {
            return _context.notifications
                .Where(n => n.UserId == userId)
                .ToList();
        }
        public void Add(Notification notification)
        {
            _context.notifications .Add(notification);
        }

        public void Update(Notification notification)
        {
            _context.notifications .Update(notification);
        }

        public Notification? GetbyNotificationId(int notificationId)
        {
            return _context.notifications.FirstOrDefault(n => n.Id == notificationId);
        }

        public int GetUnreadNotificationCount(int userId)
        {
            return _context.notifications.Where(u => u.UserId == userId && !u.IsSeen).Count();
        }
        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

    }
}
