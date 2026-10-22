
using EduHub.Ch02.Domain;
using System.Text;

namespace EduHub.Ch02.Reports;

public class CsvReportStrategy : IReportStrategy
{
    public void Generate(
        List<Club> clubs,
        Func<Guid, int> getMemberCount)
    {
        var lines = new List<string>
        {
            "ClubName,MemberCount"
        };

        foreach (var club in clubs)
        {
            int memberCount = getMemberCount(club.Id);

            string name = club.Name.Replace("\"", "\"\"");
            lines.Add($"\"{name}\",{memberCount}");
        }

        File.WriteAllLines(
            "club-report.csv",
            lines,
            Encoding.UTF8);

        Console.WriteLine("Đã xuất báo cáo ra file club-report.csv");
    }
}
