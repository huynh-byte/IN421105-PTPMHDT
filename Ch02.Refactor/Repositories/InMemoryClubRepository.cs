
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Repositories;

public class InMemoryClubRepository : IClubRepository
{
    private readonly List<Club> _clubs = new();

    public void Add(Club club)
    {
        _clubs.Add(club);
    }

    public Club? GetById(Guid clubId)
    {
        return _clubs.FirstOrDefault(c => c.Id == clubId);
    }

    public List<Club> GetAll()
    {
        return _clubs.ToList();
    }
}
