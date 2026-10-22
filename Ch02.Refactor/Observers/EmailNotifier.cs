
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Observers;

public class EmailNotifier : IEventObserver
{
    public void OnEventCreated(Event newEvent)
    {
        Console.WriteLine(
            $"[EMAIL] Thông báo sự kiện: {newEvent.Title}");
    }
}
