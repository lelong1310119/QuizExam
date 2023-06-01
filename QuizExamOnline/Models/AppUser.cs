namespace QuizExamOnline.Models
{
    public class AppUser
    {
        public long Id { get; set; } 
        public string Email { get; set; }   
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string? RefreshToken { get; set; }
        public byte[]? Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Question> Questions { get; set; }    
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<ExamHistory> ExamHistories { get;}
        public virtual ICollection<AppUserRole> AppUserRoles { get; set; }

    }
}
