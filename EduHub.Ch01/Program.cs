using System;
using System.Collections.Generic;
using EduHub.Ch01.Domain;

namespace EduHub.Ch01
{
    internal class Program
    {
        private const string ClubName = "Tech Club";
        private const string EventTitle = "Workshop C# co ban";
        private const int EventCapacity = 50;
        private const int SeparatorLength = 50;

        /// <summary>
        /// In dòng phân cách và tiêu đề của một bước.
        /// </summary>
        private static void PrintStep(string title)
        {
            Console.WriteLine(new string('=', SeparatorLength));
            Console.WriteLine(title);
            Console.WriteLine(new string('-', SeparatorLength));
        }

        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Bước 1: tạo CLB
            PrintStep("Bước 1: Tạo câu lạc bộ");
            Club club = Club.Create(ClubName);
            Console.WriteLine($"Đã tạo CLB {club.Name}, trạng thái {club.Status}");

            // Bước 2: thêm 3 thành viên
            PrintStep("Bước 2: Thêm 3 thành viên");
            List<Student> students = new List<Student>
            {
                Student.Create("Nguyễn Văn An", "an@eduhub.vn"),
                Student.Create("Trần Thị Bình", "binh@eduhub.vn"),
                Student.Create("Lê Minh Cường", "cuong@eduhub.vn")
            };
            foreach (Student student in students)
            {
                Console.WriteLine($"Thành viên: {student.FullName} ({student.Email})");
            }

            // Bước 3: tạo sự kiện sức chứa 50
            PrintStep("Bước 3: Tạo sự kiện");
            DateTime startTime = DateTime.Now.AddDays(7);
            DateTime endTime = startTime.AddHours(2);
            Event ev = club.CreateEvent(EventTitle, startTime, endTime, EventCapacity);
            Console.WriteLine($"Sự kiện {ev.Title}, sức chứa {ev.Capacity}");

            // Bước 4: 2 thành viên đăng ký (Pending) rồi Confirm
            PrintStep("Bước 4: Hai thành viên đăng ký");
            List<Registration> registrations = new List<Registration>();
            for (int i = 0; i < 2; i++)
            {
                Registration registration = ev.Register(students[i].Id);
                registrations.Add(registration);
                Console.WriteLine($"{students[i].FullName} đăng ký, trạng thái {registration.Status}");
            }

            PrintStep("Bước 5: Xác nhận phiếu");
            foreach (Registration registration in registrations)
            {
                registration.Confirm();
                Console.WriteLine($"Phiếu {registration.Id} chuyển sang {registration.Status}");
            }

            // Bước 6: 1 thành viên check-in
            List<Attendance> attendances = new List<Attendance>();
            Attendance attendance = Attendance.Create(registrations[0]);
            attendances.Add(attendance);
            Console.WriteLine($"{students[0].FullName} check-in lúc {attendance.CheckedInAt:HH:mm:ss}");

            // Bước 7: báo cáo
            PrintStep("Bước 7: Báo cáo");
            int confirmedCount = 0;
            foreach (Registration registration in registrations)
            {
                if (registration.Status == Registration.StatusConfirmed)
                {
                    confirmedCount++;
                }
            }
            Console.WriteLine($"CLB: {club.Name}");
            Console.WriteLine($"Sự kiện: {ev.Title}");
            Console.WriteLine($"Số thành viên: {students.Count}");
            Console.WriteLine($"Số phiếu đã xác nhận: {confirmedCount}/{ev.Capacity}");
            Console.WriteLine($"Sự kiện đã đầy: {ev.IsFull(confirmedCount)}");
            Console.WriteLine($"Số người check-in: {attendances.Count}");
            Console.WriteLine(new string('=', SeparatorLength));
        }
    }
}