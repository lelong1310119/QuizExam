namespace QuizExamOnline.Responses
{
    public class ResponseQuestion<T> : BaseResponse
    {
        public T Question { get; set; }

        public ResponseQuestion(string message, T info) : base(message)
        {
            Question = info;
        }
    }
}
