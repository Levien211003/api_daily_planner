namespace APIDailyPlanner.Models
{
    public class Users
    {
       
            public int UserID { get; set; } //primary
            public string Email { get; set; }
            public string Password { get; set; }
            public string StudentID { get; set; }
            public string Name { get; set; }
            public string? ProfilePicture { get; set; }
            public DateTime? LastLogin { get; set; }
            public bool IsDarkMode { get; set; }
            public string Language { get; set; }
    
    }
}
