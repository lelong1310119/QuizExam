namespace QuizExamOnline.Common
{
    public class CustomEnum : IEquatable<CustomEnum>
    {
        public long Id { get; }
        public string Message { get; }
        public string Detail { get; }
        public CustomEnum(long Id, string Message, string Detail)
        {
            this.Id = Id;
            this.Message = Message;
            this.Detail = Detail;
        }
        public bool Equals(CustomEnum other)
        {
            if (this.Id != other.Id) return false;
            if (this.Message != other.Message) return false;
            if (this.Detail != other.Detail) return false;
            return true;
        }
    }
}
