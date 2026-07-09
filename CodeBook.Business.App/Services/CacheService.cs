using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBook.Business.App.Interfaces;
using CodeBook.Data.App.IRepositories;
using CodeBook.Models.App;
using Microsoft.Extensions.Caching.Memory;

namespace CodeBook.Business.App.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly INotificationRepository _notificationRepository;

        public CacheService(IMemoryCache cache, INotificationRepository notificationRepository)
        {
            _cache = cache;
            _notificationRepository = notificationRepository;
        }

        public T Get<T>(string key)
        {
            return _cache.TryGetValue(key, out T value) ? value : default;
        }

        public void Set<T>(string key, T value, TimeSpan expiry)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry
            };
            _cache.Set(key, value, options);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public int GetNotificationBadgeCount(int userId)
        {
            string key = $"badge:{userId}";

            if(_cache.TryGetValue(key, out int count))
            {
                return count;
            }
            
            count = _notificationRepository.GetUnreadNotificationCount(userId);
            Set(key, count, TimeSpan.FromMinutes(5));
            return count;
        }

        public void SetNotificationBadgeCount(int userId, int count)
        {
            string key = $"badge:{userId}";
            Set(key, count, TimeSpan.FromMinutes(5));
        }

        public void InvalidateNotificationBadgeCount(int userId)
        {
            string key = $"badge:{userId}";
            Remove(key);
        } 
    }
}
