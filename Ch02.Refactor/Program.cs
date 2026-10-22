
using EduHub.Ch02.Logging;
using EduHub.Ch02.Notifications;
using EduHub.Ch02.Observers;
using EduHub.Ch02.Repositories;
using EduHub.Ch02.Reports;
using EduHub.Ch02.Services;

// 1. Khởi tạo logger và repository
IAppLogger logger = new FileAppLogger();

IClubRepository repository = new LoggingClubRepository(
    new InMemoryClubRepository(),
    logger);

// 2. Khởi tạo các service
var clubService = new ClubService(repository, logger);

var observers = new List<IEventObserver>
{
    new EmailNotifier(),
    new LogNotifier(logger)
};

var eventService = new EventService(
    repository,
    clubService,
    logger,
    observers);

var reportService = new ReportService(
    repository,
    clubService);

// 3. Tạo câu lạc bộ
var club = clubService.CreateClub(
    "Tech Club",
    "Câu lạc bộ công nghệ");

Console.WriteLine($"Đã tạo câu lạc bộ: {club.Name}");

// 4. Thêm thành viên
clubService.AddMemberToClub(
    club.Id,
    "Nguyen Van A",
    "a@student.dthu.edu.vn");

// 5. Tạo sự kiện - Observer tự thông báo
eventService.CreateEvent(
    club.Id,
    "Workshop SOLID",
    new DateTime(2026, 11, 20));

// 6. Xuất báo cáo - Strategy
Console.WriteLine("\n--- Báo cáo Console ---");
reportService.GenerateReport(new ConsoleReportStrategy());

Console.WriteLine("\n--- Xuất báo cáo CSV ---");
reportService.GenerateReport(new CsvReportStrategy());

// 7. Minh họa Factory Pattern
var notificationFactory = new NotificationServiceFactory();
var notificationService = notificationFactory.Create(
    NotificationChannel.Email);

notificationService.Send(
    "a@student.dthu.edu.vn",
    "Chào mừng bạn tham gia Tech Club!");

Console.WriteLine("\nHoàn thành chương trình demo.");
