namespace APIDailyPlanner.Models
{
    public class Task
    {
        public int TaskID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public string AssignedTo { get; set; }
        public string Status { get; set; } = "New";
        public int Priority { get; set; } = 1;
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }

}
