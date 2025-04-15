using Project_Management_System.DTOs;
using Project_Management_System.Models;

namespace Project_Management_System.Interfaces
{
    public interface INotification
    {
        // Create a new notification
        Task<NotificationDto> CreateNotificationAsync(NotificationDto notification);

        // Get all notifications
        Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync();

        // Get a notification by ID
        Task<NotificationDto> GetNotificationByIdAsync(int notificationId);

        // Update an existing notification
        Task<NotificationDto> UpdateNotificationAsync(int notificationId, NotificationDto notification);

        // Delete a notification
        Task<bool> DeleteNotificationAsync(int notificationId);
    }
}
