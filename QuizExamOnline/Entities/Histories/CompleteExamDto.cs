namespace QuizExamOnline.Entities.Histories
{
    public class CompleteExamDto
    {
        public long Id { get; set; }
        public long CompleteTime { get; set; }
        public List<CompleteQuestionDto> Questions { get; set; }
    }
}
