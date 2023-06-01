using QuizExamOnline.Entities.AnswerQuestions;

namespace QuizExamOnline.Entities.Questions
{
    public class UpdateQuestionDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public long SubjectId { get; set; }
        public long GradeId { get; set; }
        public long LevelId { get; set; }
        public long QuestionGroupId { get; set; }
        public long StatusId { get; set; }
        public long QuestionTypeId { get; set; }
        public List<CreateAnswerQuestionDto> CreateAnswerQuestionDtos { get; set; }
    }
}
