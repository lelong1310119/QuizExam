namespace QuizExamOnline.Models
{
    public class Role
    {
        public long Id { get; set; }    
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AppUserRole> AppUserRoles { get; set;}
    }
}
