using QuizExamOnline.Entities.AnswerQuestions;
using QuizExamOnline.Models;

namespace QuizExamOnline.Entities.Questions
{
    public class QuestionDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public long SubjectId { get; set; }
        public long GradeId { get; set; }
        public long LevelId { get; set; }
        public long QuestionGroupId { get; set; }
        public long StatusId { get; set; }
        public long QuestionTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<AnswerQuestionDto> Answers { get; set; }
    }
}
