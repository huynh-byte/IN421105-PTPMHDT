
namespace EduHub.Ch02.Logging;

public class FileAppLogger : IAppLogger
{
    private readonly string _filePath;

    public FileAppLogger(string filePath = "club-log.txt")
    {
        _filePath = filePath;
    }

    public void Log(string message)
    {
        File.AppendAllText(
            _filePath,
            $"[{DateTime.Now}] {message}{Environment.NewLine}");
    }
}
