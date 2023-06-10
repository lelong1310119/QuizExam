using QuizExamOnline.Common;

namespace QuizExamOnline.Enums
{
    public class GeneralEnum
    {
        public static CustomEnum GradeDoesNotExist = new CustomEnum(Id: 1, Message: "Grade does not exist", Detail: "GradeId truyền vào không chính xác");
        public static CustomEnum LevelDoesNotExist = new CustomEnum(Id: 2, Message: "Level does not exist", Detail: "LevelId truyền vào không chính xác");
        public static CustomEnum StatusDoesNotExist = new CustomEnum(Id: 3, Message: "Status does not exist", Detail: "StatusId truyền vào không chính xác");
        public static CustomEnum SubjectDoesNotExist = new CustomEnum(Id: 4, Message: "Subject does not exist", Detail: "SubjectId truyền vào không chính xác");
        public static CustomEnum QuestionGroupDoesNotExist = new CustomEnum(Id: 5, Message: "QuestionGroup does not exist", Detail: "QuestionGroupId truyền vào không chính xác");
        public static CustomEnum QuestionTypeNotExist = new CustomEnum(Id: 6, Message: "QuestionType does not exist", Detail: "QuestionTypeId truyền vào không chính xác");
    }
}
