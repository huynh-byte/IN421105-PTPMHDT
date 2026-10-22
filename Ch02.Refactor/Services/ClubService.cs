
using EduHub.Ch02.Domain;
using EduHub.Ch02.Logging;
using EduHub.Ch02.Repositories;

namespace EduHub.Ch02.Services;

public class ClubService
{
    private readonly IClubRepository _clubRepository;
    private readonly IAppLogger _logger;
    private readonly List<Member> _members = new();

    public ClubService(
        IClubRepository clubRepository,
        IAppLogger logger)
    {
        _clubRepository = clubRepository;
        _logger = logger;
    }

    public Club CreateClub(string name, string description)
    {
        var club = Club.Create(name, description);

        _clubRepository.Add(club);
        _logger.Log($"Club created: {club.Name}");

        return club;
    }

    public Member AddMemberToClub(
        Guid clubId,
        string memberName,
        string email)
    {
        var club = _clubRepository.GetById(clubId)
            ?? throw new InvalidOperationException("Club not found");

        int memberCount = _members.Count(m => m.ClubId == clubId);

        if (!club.HasRoom(memberCount))
        {
            throw new InvalidOperationException(
                "Club is full (max 50 members)");
        }

        var member = new Member
        {
            Id = Guid.NewGuid(),
            Name = memberName,
            Email = email,
            ClubId = clubId
        };

        _members.Add(member);
        _logger.Log($"Member {memberName} joined {club.Name}");

        return member;
    }

    public List<Member> GetMembersByClub(Guid clubId)
    {
        return _members
            .Where(m => m.ClubId == clubId)
            .ToList();
    }

    public List<Member> GetAllMembers()
    {
        return _members.ToList();
    }
}
