namespace APIDailyPlanner.Models
{
    public class TaskReminder
    {
        public int ReminderID { get; set; }  // Khóa chính
        public int TaskID { get; set; }
        public DateTime ReminderTime { get; set; }  // Thay đổi từ TimeSpan sang DateTime
        public bool IsNotified { get; set; }
    }
}
