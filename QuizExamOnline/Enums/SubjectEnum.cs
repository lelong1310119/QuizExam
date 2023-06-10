using TrueSight.Common;

namespace QuizExamOnline.Enums
{
    public class SubjectEnum
    {
        public static GenericEnum Maths = new GenericEnum(Id: 1, Code: "Subject_1", Name: "Toán");
        public static GenericEnum Literature = new GenericEnum(Id: 2, Code: "Subject_2", Name: "Văn");
        public static GenericEnum English = new GenericEnum(Id: 3, Code: "Subject_3", Name: "Anh");
        public static GenericEnum History = new GenericEnum(Id: 4, Code: "Subject_4", Name: "Sử");
        public static GenericEnum Geography = new GenericEnum(Id: 5, Code: "Subject_5", Name: "Địa");
        public static GenericEnum Chemistry = new GenericEnum(Id: 6, Code: "Subject_6", Name: "Hóa");
        public static GenericEnum Physical = new GenericEnum(Id: 7, Code: "Subject_7", Name: "Lý");

        public static List<GenericEnum> SubjectEnumList = new List<GenericEnum>
        {
            Maths, Literature, English, History, Geography, Chemistry, Physical
        };
    }
}
