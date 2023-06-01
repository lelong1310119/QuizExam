namespace QuizExamOnline.Models
{
    public class Level
    {
        public Level() {
            Questions = new HashSet<Question>();
            Exams = new HashSet<Exam>();
        }  

        public long Id { get; set; }    
        public string Code { get; set; }    
        public string Name { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
    }
}
