using System;

namespace CodeBook.Business.App.Interfaces
{
    public interface ICacheService
    {
        T Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan expiry);
        void Remove(string key);
        int GetNotificationBadgeCount(int userId);
        void SetNotificationBadgeCount(int userId, int count);
        void InvalidateNotificationBadgeCount(int userId);
    }
}
