namespace QuizExamOnline.Models
{
    public class AnswerQuestion { 
        public long Id { get; set; }
        public string Content { get; set; } 
        public Boolean IsRight { get; set; }
        public long QuestionId { get; set; }
     
        public virtual Question Question { get; set; }  
    }
}
