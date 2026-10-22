
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Reports;

public class ConsoleReportStrategy : IReportStrategy
{
    public void Generate(
        List<Club> clubs,
        Func<Guid, int> getMemberCount)
    {
        Console.WriteLine("===== BÁO CÁO CÂU LẠC BỘ =====");

        foreach (var club in clubs)
        {
            int memberCount = getMemberCount(club.Id);

            Console.WriteLine(
                $"Tên CLB: {club.Name} | Số thành viên: {memberCount}");
        }
    }
}
