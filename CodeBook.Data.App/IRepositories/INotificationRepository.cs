using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBook.Models.App;

namespace CodeBook.Data.App.IRepositories
{
    public interface INotificationRepository
    {
        List<Notification> GetNotificationsbyUserId(int userId);
        void Add(Notification notification);
        void Update(Notification notification);
        Notification GetbyNotificationId(int notificationId);

        int GetUnreadNotificationCount(int userId);
        bool SaveChanges();
    }
}
