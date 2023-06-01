using QuizExamOnline.Entities.Questions;

namespace QuizExamOnline.Entities.Exams
{
    public class CreateExamDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public byte[]? Image { get; set; }
        public long SubjectId { get; set; }
        public long GradeId { get; set; }
        public long LevelId { get; set; }
        public long StatusId { get; set; }
        public long Time { get; set; }
        public List<long> IdQuestions { get; set; }
    }
}
