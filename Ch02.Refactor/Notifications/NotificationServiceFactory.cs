
namespace EduHub.Ch02.Notifications;

public class NotificationServiceFactory
{
    public INotificationService Create(NotificationChannel channel)
    {
        switch (channel)
        {
            case NotificationChannel.Email:
            case NotificationChannel.Gmail:
            case NotificationChannel.Yahoo:
                return new EmailNotificationService();

            default:
                throw new ArgumentException("Kênh thông báo không hợp lệ");
        }
    }
}
