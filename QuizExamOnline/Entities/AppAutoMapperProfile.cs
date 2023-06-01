using AutoMapper;
using QuizExamOnline.Entities.AnswerQuestions;
using QuizExamOnline.Entities.AppUsers;
using QuizExamOnline.Entities.Exams;
using QuizExamOnline.Entities.Histories;
using QuizExamOnline.Entities.Questions;
using QuizExamOnline.Models;

namespace QuizExamOnline.Entities
{
    public class AppAutoMapperProfile : Profile
    {
        public AppAutoMapperProfile() {
            CreateMap<CreateAppUserDto, AppUser>();
            CreateMap<AppUser, AppUserDto>();
            CreateMap<Grade, EntityEnumDto>();
            CreateMap<Subject, EntityEnumDto>();
            CreateMap<Level, EntityEnumDto>();
            CreateMap<Status, EntityEnumDto>();
            CreateMap<QuestionGroup, EntityEnumDto>();
            CreateMap<QuestionType, EntityEnumDto>();
            CreateMap<Grade, EntityEnumDto>();
            CreateMap<Grade, EntityEnumDto>();
            CreateMap<Grade, EntityEnumDto>();
            CreateMap<Question, QuestionDto>();
            CreateMap<CreateQuestionDto, Question>();
            CreateMap<UpdateQuestionDto, Question>();
            CreateMap<AnswerQuestion, AnswerQuestionDto>();
            CreateMap<CreateAnswerQuestionDto, AnswerQuestion>();
            CreateMap<Exam, ExamDto>();
            CreateMap<CreateExamDto, Exam>();
            CreateMap<UpdateExamDto, Exam>();
            CreateMap<ExamHistoryDto, ExamHistory>();
            CreateMap<ExamHistory, ExamHistoryDto>();
            CreateMap<MyHistoryDto, ExamHistory>();
            CreateMap<ExamHistory, MyHistoryDto>();
            CreateMap<CreateHistoryExamDto, ExamHistory>();
            CreateMap<UpdateQuestionDto, CreateQuestionDto>();
        }
    }
}
