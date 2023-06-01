namespace QuizExamOnline.Models
{
    public class AppUserRole
    {
        public long AppUserId { get; set; }
        public long RoleId { get; set; }    

        public virtual AppUser AppUser { get; set; }
        public virtual Role Role { get; set; }
    }
}
