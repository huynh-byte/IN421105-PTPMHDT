
using EduHub.Ch02.Domain;
using EduHub.Ch02.Logging;
using EduHub.Ch02.Observers;
using EduHub.Ch02.Repositories;

namespace EduHub.Ch02.Services;

public class EventService
{
    private readonly IClubRepository _clubRepository;
    private readonly ClubService _clubService;
    private readonly IAppLogger _logger;
    private readonly IEnumerable<IEventObserver> _observers;

    public EventService(
        IClubRepository clubRepository,
        ClubService clubService,
        IAppLogger logger,
        IEnumerable<IEventObserver> observers)
    {
        _clubRepository = clubRepository;
        _clubService = clubService;
        _logger = logger;
        _observers = observers;
    }

    public Event CreateEvent(Guid clubId, string title, DateTime date)
    {
        var club = _clubRepository.GetById(clubId)
            ?? throw new InvalidOperationException("Club not found");

        var newEvent = new Event
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Title = title,
            Date = date
        };

        _logger.Log($"Event created: {title} for club {club.Name}");

        foreach (var member in _clubService.GetMembersByClub(clubId))
        {
            newEvent.Register(member.Id);
        }

        foreach (var observer in _observers)
        {
            observer.OnEventCreated(newEvent);
        }

        return newEvent;
    }
}
