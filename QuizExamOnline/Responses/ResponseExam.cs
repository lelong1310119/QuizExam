namespace QuizExamOnline.Responses
{
    public class ResponseExam<T> : BaseResponse
    {
        public T Exam { get; set; }

        public ResponseExam(string message, T info) : base(message)
        {
            Exam = info;
        }
    }
}
