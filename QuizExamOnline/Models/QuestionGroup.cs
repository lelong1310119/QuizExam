namespace QuizExamOnline.Models
{
    public class QuestionGroup
    {
        public QuestionGroup() { 
            Questions = new HashSet<Question>();
        }  

        public long Id { get; set; }
        public string Code { get; set; }    
        public string Name { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
