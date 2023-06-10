using QuizExamOnline.Common;

namespace QuizExamOnline.Enums
{
    public class UserErrorEnum
    {
        public static CustomEnum EmailDoesNotExist = new CustomEnum(Id: 1, Message: "Email does not exist", Detail: "Email này chưa được đăng ký tài khoản");
        public static CustomEnum EmailEmpty = new CustomEnum(Id: 2, Message: "Email empty", Detail: "Email trống hoặc chỉ chứa khoảng trăng");
        public static CustomEnum InvalidEmail = new CustomEnum(Id: 3, Message: "Invalid email", Detail: "Email này chưa được đăng ký tài khoản");
        public static CustomEnum EmailAlreadyExists = new CustomEnum(Id: 4, Message: "Email already exists", Detail: "Email này đã được dùng để đăng ký tài khoản");
        public static CustomEnum InvalidDisplayname = new CustomEnum(Id: 5, Message: "Invalid displayname", Detail: "Displayname không quá 100 kí tự");
        public static CustomEnum DisplaynameEmpty = new CustomEnum(Id: 6, Message: "Displayname empty", Detail: "Displayname trống hoặc chỉ chứa khoảng trắng");
        public static CustomEnum PasswordEmpty = new CustomEnum(Id: 7, Message: "Password empty", Detail: "Password rỗng hoặc chỉ chứa khoảng trắng");
        public static CustomEnum InvalidPassword = new CustomEnum(Id: 8, Message: "Invalid password", Detail: "Password không được chứa các khoảng trắng và có ít nhất 8 kí tự");
        public static CustomEnum IncorrectPassword = new CustomEnum(Id: 9, Message: "Incorrect password", Detail: "Password không chính xác");
        public static CustomEnum RefreshTokenEmtpy = new CustomEnum(Id: 10, Message: "RefreshToken empty", Detail: "RefreshToken trống hoặc chỉ chứa khoảng trắng");
        public static CustomEnum InvalidRefreshToken = new CustomEnum(Id: 11, Message: "Invalid RefreshToken", Detail: "Refreshtoken không chính xác hoặc đã quá hạn");
        public static CustomEnum IncorrectRefreshToken = new CustomEnum(Id: 12, Message: "Incorrect RefreshToken", Detail: "Refreshtoken không chính xác");
    }
}
