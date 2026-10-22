
using EduHub.Ch02.Domain;
using EduHub.Ch02.Logging;

namespace EduHub.Ch02.Observers;

public class LogNotifier : IEventObserver
{
    private readonly IAppLogger _logger;

    public LogNotifier(IAppLogger logger)
    {
        _logger = logger;
    }

    public void OnEventCreated(Event newEvent)
    {
        _logger.Log($"Event created: {newEvent.Title}");
    }
}
