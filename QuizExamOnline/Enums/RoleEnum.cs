using TrueSight.Common;

namespace QuizExamOnline.Enums
{
    public class RoleEnum
    {
        public static GenericEnum Admin = new GenericEnum(Id: 1, Code: "Role_1", Name: "Admin");
        public static GenericEnum User = new GenericEnum(Id: 2, Code: "Role_2", Name: "User");

        public static List<GenericEnum> RoleEnumList = new List<GenericEnum>
        {
            Admin, User
        };
    }
}
