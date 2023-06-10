using TrueSight.Common;

namespace QuizExamOnline.Enums
{
    public class QuestionGroupEnum
    {
        public static GenericEnum Group_1 = new GenericEnum(Id: 1, Code: "QuestionGroup_1", Name: "Câu hỏi nhận biết");
        public static GenericEnum Group_2 = new GenericEnum(Id: 2, Code: "QuestionGroup_2", Name: "Câu hỏi hiểu");
        public static GenericEnum Group_3 = new GenericEnum(Id: 3, Code: "QuestionGroup_3", Name: "Câu hỏi vận dụng");
        public static GenericEnum Group_4 = new GenericEnum(Id: 4, Code: "QuestionGroup_4", Name: "Câu hỏi vận dụng cao");

        public static List<GenericEnum> QuestionGroupEnumList = new List<GenericEnum>
        {
            Group_1, Group_2, Group_3, Group_4,
        };
    }
}
