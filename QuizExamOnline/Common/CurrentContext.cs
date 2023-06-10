using TrueSight.Common;

namespace QuizExamOnline.Common
{
    public interface ICurrentContext
    {
        long UserId { get; set; }
        string DisplayName { get; set; }
        string Token { get; set; }
        string Action { get; set; }
        string Message { get; set; }
        string Detail { get; set; }
    }

    public class CurrentContext : ICurrentContext
    {
        public long UserId { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
    }
}

