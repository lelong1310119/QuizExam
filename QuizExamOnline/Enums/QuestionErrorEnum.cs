using QuizExamOnline.Common;

namespace QuizExamOnline.Enums
{
    public class QuestionErrorEnum
    {
        public static CustomEnum QuestionDoesNotExist = new CustomEnum(Id: 1, Message: "Question does not exist", Detail: "Id Question truyền vào không tồn tại");
        public static CustomEnum AnswerEmpty = new CustomEnum(Id: 2, Message: "AnswerEmpty", Detail: "Danh sách answer trống");
        public static CustomEnum CanNotChange = new CustomEnum(Id: 3, Message: "Can not change", Detail: "Không có quyền thay đỗi dữ liệu này");
        public static CustomEnum ContentEmpty = new CustomEnum(Id: 4, Message: "Content empty", Detail: "Content rỗng hoặc chỉ chứa các khoảng trắng");
        public static CustomEnum InvalidAnswer = new CustomEnum(Id: 5, Message: "Invalid Answer", Detail: "Question truyền vào yêu cầu một đáp án đúng với câu hỏi một đáp án và ít nhất hai đáp án đúng với câu hỏi nhiều đáp án");
    }
}
