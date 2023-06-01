using QuizExamOnline.Entities.Questions;

namespace QuizExamOnline.Entities.Exams
{
    public class ExamDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public byte[]? Image { get; set; }
        public long SubjectId { get; set; }
        public long GradeId { get; set; }
        public long LevelId { get; set; }
        public long StatusId { get; set; }
        public long Time { get; set; }
        public int? TimeExam { get; set; }  
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }
}
