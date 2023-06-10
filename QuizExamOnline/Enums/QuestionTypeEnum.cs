using TrueSight.Common;

namespace QuizExamOnline.Enums
{
    public class QuestionTypeEnum
    {
        public static GenericEnum Type_1 = new GenericEnum(Id: 1, Code: "QuestionType_1", Name: "Câu hỏi một đáp án");
        public static GenericEnum Type_2 = new GenericEnum(Id: 2, Code: "QuestionType_2", Name: "Câu hỏi nhiều đáp án");

        public static List<GenericEnum> QuestionTypeEnumList = new List<GenericEnum>
        {
            Type_1, Type_2
        };
    }
}
