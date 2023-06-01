namespace QuizExamOnline.Responses
{
    public class ResponseUser<T> : BaseResponse
    {
        public T User { get; set; }

        public ResponseUser(string message, T info) : base(message)
        {
           User = info;
        }
    }
}