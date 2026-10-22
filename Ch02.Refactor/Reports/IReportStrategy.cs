
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Reports;

public interface IReportStrategy
{
    void Generate(List<Club> clubs, Func<Guid, int> getMemberCount);
}
