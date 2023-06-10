using QuizExamOnline.Common;

namespace QuizExamOnline.Enums
{
    public class ExamErrorEnum
    {
        public static CustomEnum ExamDoesNotExist = new CustomEnum(Id: 1, Message: "Exam does not exist", Detail: "Id của Exam truyền vào không tồn tại");
        public static CustomEnum NotFoundExam = new CustomEnum(Id: 2, Message: "Not found exam", Detail: "Không thể tìm thấy exam");
        public static CustomEnum CanNotChange = new CustomEnum(Id: 3, Message: "Can not change", Detail: "Không có quyền thay đỗi dữ liệu này");
        public static CustomEnum NameEmpty = new CustomEnum(Id: 4, Message: "Name empty", Detail: "Name rỗng hoặc chỉ chứa các khoảng trắng");
        public static CustomEnum InvalidCode = new CustomEnum(Id: 5, Message: "Invalid code", Detail: "Code không được rỗng và không có khoảng trắng");
        public static CustomEnum QuestionEmpty = new CustomEnum(Id: 6, Message: "Question empty", Detail: "Chưa có question nào được thêm vào");
        public static CustomEnum InvalidTime = new CustomEnum(Id: 7, Message: "Invalid time", Detail: "Time truyền vào phải là số dương");
        public static CustomEnum QuestionDoesNotExits = new CustomEnum(Id: 8, Message: "Question does not exist", Detail: "QuestionTypeId truyền vào không chính xác");
        public static CustomEnum QuestionDoesNotMapExam = new CustomEnum(Id: 9, Message: "Question does not map exam", Detail: "IdQuestion không có trong Exam");
        public static CustomEnum InvalidQuestion = new CustomEnum(Id: 10, Message: "Invalid Question", Detail: "Câu hỏi một đáp án chỉ được gửi nhiều hơn một đáp án");
        public static CustomEnum AnswerDoesNotMapQuestion = new CustomEnum(Id: 11, Message: "Answer does not map question", Detail: "IdAnswer không có trong Question hoặc không tồn tại");
        public static CustomEnum CodeAlreadyExists = new CustomEnum(Id: 12, Message: "Code already exists", Detail: "Code đã tồn tại");
        public static CustomEnum InvalidPage = new CustomEnum(Id: 13, Message: "Invalid Page", Detail: "Page truyền vào không hợp lệ");

    }
}
