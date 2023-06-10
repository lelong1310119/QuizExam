using TrueSight.Common;

namespace QuizExamOnline.Enums
{
    public class LevelEnum
    {
        public static GenericEnum Easy = new GenericEnum(Id: 1, Code: "Level_1", Name: "Dễ");
        public static GenericEnum Normal = new GenericEnum(Id: 2, Code: "Level_2", Name: "Trung bình");
        public static GenericEnum Hard = new GenericEnum(Id: 3, Code: "Level_3", Name: "Khó");

        public static List<GenericEnum> LevelEnumList = new List<GenericEnum>
        {
            Easy, Normal, Hard
        };
    }
}
