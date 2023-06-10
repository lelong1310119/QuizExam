using QuizExamOnline.Common;

namespace QuizExamOnline.Responses
{
    public class ResponseException : BaseResponse
    {
        public int StatusCode { get; set; }
        public string Type { get; set; }
        public string Detail { get; set; }
        public ResponseException(int statusCode, string message, string detail, string type) : base(message)
        {
            StatusCode = statusCode;
            Type = type;
            Detail = detail;
        }
        public ResponseException(int statusCode, CustomEnum customEnum, string type) : base(customEnum.Message)
        {
            StatusCode = statusCode;
            Detail = customEnum.Detail;
            Type = type;
        }
    }
}
