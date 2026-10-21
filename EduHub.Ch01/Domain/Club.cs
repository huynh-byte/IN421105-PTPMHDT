using System;
namespace EduHub.Ch01.Domain{
    /// <summary>
    /// Câu lạc bộ tổ chức sự kiện.
    /// </summary>
    public class Club{
       public const string StatusActive = "Active";
        public const string StatusSuspended = "Suspended";

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Status { get; private set; }
        public string SuspendReason { get; private set; }

        private Club(Guid id, string name){
            Id = id;
            Name = name;
            Status = StatusActive;
            SuspendReason = "";
        }
        /// <summary>
        /// Tạo câu lạc bộ mới, kiểm tra tên không được trống.
        /// </summary>

        public static Club Create(string name){
            if (string.IsNullOrWhiteSpace(name))
                 throw new ArgumentException("Tên câu lạc bộ không được để trống.");
            
            return new Club(Guid.NewGuid(), name.Trim()); 
        }

        /// <summary>
        /// Tạm khoá câu lạc bộ kèm lý do.
        /// </summary>

        public void Suspend(string reason){
            if (Status == StatusSuspended)
                throw new InvalidOperationException("Câu lạc bộ đã bị khoá.");
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Phải ghi lý do khoá.");

            Status = StatusSuspended;
            SuspendReason = reason.Trim();
        }

        /// <summary>
        /// Tạo sự kiện mới, không được khi câu lạc bộ đang bị khoá.
        /// </summary>

        public Event CreateEvent(string title, DateTime startTime, DateTime endTime, int capacity){
            if (Status == StatusSuspended)
                throw new InvalidOperationException("Câu lạc bộ đang bị khoá, không thể tạo sự kiện.");

            return Event.Create(Id, title, startTime, endTime, capacity);
        }
 
    }
}