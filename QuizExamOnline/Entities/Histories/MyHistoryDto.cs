using QuizExamOnline.Entities.Exams;
using QuizExamOnline.Models;

namespace QuizExamOnline.Entities.Histories
{
    public class MyHistoryDto
    {
        public long Id { get; set; }
        public long ExamId { get; set; }
        public DateTime CreateAt { get; set; }
        public long CompleteTime { get; set; }
        public double QuestionRight { get; set; }
        public double ToTalScore { get; set; }

        public string NameExam { get; set; }
    }
}
