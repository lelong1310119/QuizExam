namespace QuizExamOnline.Models
{
    public class Exam
    {
        public long Id { get; set; }    
        public string Name { get; set; }
        public string Code { get; set; }
        public byte[]? Image { get; set; }
        public long SubjectId { get; set; }
        public long GradeId { get; set; }
        public long LevelId { get; set; }
        public long StatusId { get; set; }
        public long AppUserId { get; set; }
        public long Time { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual Level Level { get; set; }
        public virtual Status Status { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }    
        public virtual ICollection<ExamHistory> ExamHistories { get; set; }
    }
}
