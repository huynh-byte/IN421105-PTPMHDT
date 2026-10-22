
using EduHub.Ch02.Domain;
using EduHub.Ch02.Reports;
using EduHub.Ch02.Repositories;

namespace EduHub.Ch02.Services;

public class ReportService
{
    private readonly IClubRepository _clubRepository;
    private readonly ClubService _clubService;

    public ReportService(
        IClubRepository clubRepository,
        ClubService clubService)
    {
        _clubRepository = clubRepository;
        _clubService = clubService;
    }

    public void GenerateReport(IReportStrategy strategy)
    {
        List<Club> clubs = _clubRepository.GetAll();

        strategy.Generate(
            clubs,
            clubId => _clubService.GetMembersByClub(clubId).Count);
    }
}
