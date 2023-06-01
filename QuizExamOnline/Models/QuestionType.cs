namespace QuizExamOnline.Models
{
    public class QuestionType
    {
        public QuestionType() { 
            Questions = new HashSet<Question>();    
        }   

        public long Id { get; set; }    
        public string Code { get; set; }    
        public string Name { get; set; }    

        public virtual ICollection<Question> Questions { get; set;}
    }
}
