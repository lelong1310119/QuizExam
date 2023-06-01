using QuizExamOnline.Models;

namespace QuizExamOnline.Entities.Histories
{
    public class CreateHistoryExamDto
    {
        public long ExamId { get; set; }
        public long CompleteTime { get; set; }
        public double QuestionRight { get; set; }
        public double ToTalScore { get; set; }
    }
}
