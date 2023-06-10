namespace QuizExamOnline.Common
{
    public class CustomException : Exception
    {
        public string Detail { get; set; }
        public CustomException(CustomEnum enumCustom) : base(enumCustom.Message) {
            Detail = enumCustom.Detail;
        }
    }
}
