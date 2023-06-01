namespace QuizExamOnline.Models
{
    public class ExamHistory
    {
        public long Id { get; set; }    
        public long AppUserId { get; set; }
        public long ExamId { get; set; }
        public DateTime CreateAt { get; set; }
        public long CompleteTime { get; set; }
        public double QuestionRight { get; set; }   
        public double ToTalScore { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual Exam Exam { get; set; }  
    }
}
