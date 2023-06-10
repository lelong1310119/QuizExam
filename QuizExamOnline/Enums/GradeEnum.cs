using TrueSight.Common;

namespace QuizExamOnline.Enums
{
    public class GradeEnum
    {
        public static GenericEnum Grade_10 = new GenericEnum(Id: 1, Code: "Grade_1", Name: "Lớp 10");
        public static GenericEnum Grade_11 = new GenericEnum(Id: 2, Code: "Grade_2", Name: "Lớp 11");
        public static GenericEnum Grade_12 = new GenericEnum(Id: 3, Code: "Grade_3", Name: "Lớp 12");

        public static List<GenericEnum> GradeEnumList = new List<GenericEnum>
        {
            Grade_10, Grade_11, Grade_12
        };
    }
}
