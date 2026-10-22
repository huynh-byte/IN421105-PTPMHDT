
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Observers;

public interface IEventObserver
{
    void OnEventCreated(Event newEvent);
}
