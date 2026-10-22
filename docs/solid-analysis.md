# Phân tích vi phạm SOLID trong ClubManager


## 1. Single Responsibility Principle (SRP) — tạo câu lạc bộ
**Vị trí:** `ClubManager.cs` — phương thức `CreateClub()` (dòng 35–64).

**Vi phạm:** Một phương thức làm 4 việc: validate (dòng 38–41), lưu dữ liệu (dòng 44–45), ghi log (dòng 48) và gửi thông báo (dòng 52–63).

**Hậu quả:** Đổi quy tắc validate hay cách gửi thông báo đều phải sửa chung một chỗ. Không thể kiểm thử riêng từng việc, một lỗi ở phần thông báo có thể làm hỏng cả luồng tạo CLB.

**Hướng khắc phục:** Giao nghiệp vụ cho `ClubService`; giao ghi log cho `IAppLogger` và gửi thông báo cho `INotificationService`.

## 2. Open/Closed Principle (OCP) — chọn kênh thông báo
**Vị trí:** `ClubManager.cs` — phương thức `CreateClub()`, chuỗi `if/else` theo email (dòng 52–63).

**Vi phạm:** Chọn cách gửi bằng `Contains("@gmail.com")` và `Contains("@yahoo.com")`. Khi cần thêm kênh mới như SMS phải sửa trực tiếp phương thức đang chạy.

**Hậu quả:** Dễ gây lỗi hồi quy cho các nhánh cũ. Nhận diện bằng chuỗi cũng không đáng tin, ví dụ `a@gmail.com.vn` vẫn khớp nhánh Gmail.

**Hướng khắc phục:** Dùng Factory (`NotificationServiceFactory`) để tạo `INotificationService` theo `NotificationChannel`.

## 3. Dependency Inversion Principle (DIP) — ghi log
**Vị trí:** `ClubManager.cs` — các lệnh `File.AppendAllText()` ở dòng 48, 79, 94, 111 và đường dẫn cứng `"club-log.txt"` ở dòng 32.

**Vi phạm:** Lớp nghiệp vụ phụ thuộc trực tiếp vào file system thay vì phụ thuộc vào interface.

**Hậu quả:** Unit test ghi ra file thật trên đĩa. Không thể đổi sang DB hay console, và không kiểm chứng được nội dung log trong test.

**Hướng khắc phục:** Tạo `IAppLogger` (triển khai `FileAppLogger`) và truyền vào qua constructor.

## 4. Dependency Inversion Principle (DIP) + Open/Closed Principle (OCP) — thông báo khi thêm thành viên
**Vị trí:** `ClubManager.cs` — phương thức `AddMemberToClub()`, các lệnh `Console.WriteLine` (dòng 82–83)
.
**Vi phạm:** Việc gửi email chào mừng và thông báo Slack được viết cứng bằng `Console.WriteLine` trong phương thức nghiệp vụ.

**Hậu quả:** Không thể tắt, thay thế hay giả lập thông báo khi kiểm thử. Thêm kênh mới phải sửa lại phương thức.

**Hướng khắc phục:** Gọi `INotificationService` lấy từ Factory; `ClubService` chỉ xử lý nghiệp vụ thêm thành viên.

## 5. Single Responsibility Principle (SRP) + Open/Closed Principle (OCP) — tạo sự kiện
**Vị trí:** `ClubManager.cs` — phương thức `CreateEvent()` (dòng 88–99), đoạn `foreach` gửi mail (dòng 96–97).

**Vi phạm:** Phương thức tạo sự kiện nhưng cũng tự lặp qua từng thành viên để gửi mail, nên nghiệp vụ dính chặt với kênh thông báo.

**Hậu quả:** Thêm người nhận mới (ghi log, báo admin) phải sửa chính phương thức này.

**Hướng khắc phục:** Tạo `EventService` và áp dụng Observer: `IEventObserver` với `EmailNotifier`, `LogNotifier`; `EventService` chỉ gọi `OnEventCreated`.

## 6. Single Responsibility Principle (SRP) + Open/Closed Principle (OCP) — tạo báo cáo
**Vị trí:** `ClubManager.cs` — phương thức `PrintClubReport()` (dòng 103–113).

**Vi phạm:** Trộn truy vấn dữ liệu, định dạng cứng bằng `Console.WriteLine` (dòng 108) và ghi log (dòng 111). Muốn xuất CSV phải sửa phương thức hiện có.

**Hậu quả:** Mỗi định dạng báo cáo mới đều chạm vào code cũ. Nội dung báo cáo chỉ in ra console nên không thể kiểm thử bằng assert.

**Hướng khắc phục:** Tạo `ReportService` và áp dụng Strategy: `IReportStrategy` với `ConsoleReportStrategy`, `CsvReportStrategy`.

## 7. Dependency Inversion Principle (DIP) + đóng gói — lưu trữ dữ liệu
**Vị trí:** `ClubManager.cs` — `_clubs`, `_members` (dòng 30–31) và `GetAllClubs()` (dòng 116).

**Vi phạm:** Dữ liệu là `List` nội bộ, `GetAllClubs()` trả về chính list đó. Không có `IClubRepository` làm lớp trung gian.

**Hậu quả:** Bên ngoài có thể `Add` hay `Clear` trực tiếp, phá vỡ quy tắc như giới hạn 50 thành viên. Không thể thay bằng DB hay gắn log truy cập.

**Hướng khắc phục:** Tạo `IClubRepository` (và `IMemberRepository`), trả về `IReadOnlyList`; bọc bằng `LoggingClubRepository` theo Decorator.

## 8. Interface Segregation Principle (ISP) — API quá rộng
**Vị trí:** `ClubManager.cs` — toàn bộ class (dòng 28–117).

**Vi phạm:** Client chỉ cần đọc danh sách CLB vẫn phải phụ thuộc cả `ClubManager`, gồm tạo sự kiện và in báo cáo.

**Hậu quả:** Không thể mock riêng từng nhóm chức năng, và mọi client bị ảnh hưởng khi một nhóm chức năng thay đổi.

**Hướng khắc phục:** Tách thành các interface nhỏ theo nhóm chức năng (repository, logger, notification, report).

## 9. Lỗi logic liên quan — sự kiện không được lưu
**Vị trí:** `ClubManager.cs` — phương thức `CreateEvent()` (dòng 93–98).

**Vi phạm:** Tạo biến `ev` rồi trả về nhưng không lưu vào đâu. Đây không phải vi phạm SOLID thuần túy, nhưng là hệ quả của việc thiếu repository.

**Hậu quả:** Sự kiện mất ngay sau khi phương thức kết thúc, không thể truy vấn lại.

**Hướng khắc phục:** Hiện tại, `EventService` tạo sự kiện và thông báo qua các observer nhưng chưa lưu sự kiện vào repository. Có thể bổ sung `IEventRepository` nếu cần truy vấn lại sự kiện sau khi tạo.


