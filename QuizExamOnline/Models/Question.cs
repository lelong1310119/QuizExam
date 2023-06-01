using Microsoft.Identity.Client;

namespace QuizExamOnline.Models
{
    public class Question
    {
        public long Id { get; set; }    
        public string Content { get; set; } 
        public long SubjectId { get; set; }
        public long GradeId { get; set; }
        public long LevelId { get; set; }
        public long QuestionGroupId { get; set; }   
        public long StatusId { get; set; }
        public long QuestionTypeId { get; set; } 
        public long AppUserId { get; set; }
        public DateTime CreatedAt { get; set; }  
        public DateTime UpdatedAt { get; set; } 

        public virtual Subject Subject { get; set; }    
        public virtual Grade Grade { get; set; }
        public virtual Level Level { get; set; }
        public virtual QuestionGroup QuestionGroup { get; set; }
        public virtual Status Status { get; set; }  
        public virtual AppUser AppUser { get; set; }
        public virtual QuestionType QuestionType { get; set; }  
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        public virtual ICollection<AnswerQuestion> AnswerQuestions { get; set; }

    }
}
