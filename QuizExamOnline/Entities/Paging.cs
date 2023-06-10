namespace QuizExamOnline.Entities
{
    public class Paging<T>
    {
        public int TotalPage { get; set; }
        public List<T> Data { get; set; }
    }
}
