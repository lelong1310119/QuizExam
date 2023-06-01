namespace QuizExamOnline.Models
{
    public class ExamQuestion
    {
        public long ExamId { get; set; }
        public long QuestionId { get; set; }  
        public double Mark { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual Question Question { get; set; }
    }
}
