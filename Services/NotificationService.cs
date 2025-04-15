using Project_Management_System.DTOs;
using Project_Management_System.Interfaces;

namespace Project_Management_System.Services
{
    public class NotificationService : INotification
    {
        public Task<NotificationDto> CreateNotificationAsync(NotificationDto notification)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteNotificationAsync(int notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<NotificationDto> GetNotificationByIdAsync(int notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationDto> UpdateNotificationAsync(int notificationId, NotificationDto notification)
        {
            throw new NotImplementedException();
        }
    }
}
