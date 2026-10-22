
using EduHub.Ch02.Domain;
using EduHub.Ch02.Logging;

namespace EduHub.Ch02.Repositories;

public class LoggingClubRepository : IClubRepository
{
    private readonly IClubRepository _inner;
    private readonly IAppLogger _logger;

    public LoggingClubRepository(
        IClubRepository inner,
        IAppLogger logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public void Add(Club club)
    {
        _inner.Add(club);
        _logger.Log($"Added club: {club.Name}");
    }

    public Club? GetById(Guid clubId)
    {
        _logger.Log($"Finding club: {clubId}");
        return _inner.GetById(clubId);
    }

    public List<Club> GetAll()
    {
        _logger.Log("Getting all clubs");
        return _inner.GetAll();
    }
}
