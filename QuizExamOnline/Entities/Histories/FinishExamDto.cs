namespace QuizExamOnline.Entities.Histories
{
    public class FinishExamDto
    {
        public long Id { get; set; }    
        public string Name { get; set; }
        public int TotalQuestion { get; set; }
        public DateTime CreateAt { get; set; }
        public long CompleteTime { get; set; }
        public double QuestionRight { get; set; }
        public double ToTalScore { get; set; }
    }
}
