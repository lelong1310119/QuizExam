using QuizExamOnline.Entities.AppUsers;
using QuizExamOnline.Models;

namespace QuizExamOnline.Entities.Histories
{
    public class ExamHistoryDto
    {
        public long Id { get; set; }
        public long AppUserId { get; set; }
        public DateTime CreateAt { get; set; }
        public long CompleteTime { get; set; }
        public double QuestionRight { get; set; }
        public double ToTalScore { get; set; }

        public AppUserDto AppUser { get; set; }
    }
}
