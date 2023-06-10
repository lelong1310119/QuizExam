using TrueSight.Common;

namespace QuizExamOnline.Enums
{
    public class StatusEnum
    {
        public static GenericEnum Public = new GenericEnum(Id: 1, Code: "Status_1", Name: "Public");
        public static GenericEnum Private = new GenericEnum(Id: 2, Code: "Status_2", Name: "Private");
        public static GenericEnum Draft = new GenericEnum(Id: 3, Code: "Status_3", Name: "Draft");

        public static List<GenericEnum> StatusEnumList = new List<GenericEnum>
        {
            Public, Private, Draft
        };
    }
}
