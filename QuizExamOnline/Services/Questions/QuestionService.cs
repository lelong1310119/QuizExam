using Microsoft.CodeAnalysis.CSharp.Syntax;
using Nest;
using QuizExamOnline.Common;
using QuizExamOnline.Entities;
using QuizExamOnline.Entities.AnswerQuestions;
using QuizExamOnline.Entities.Exams;
using QuizExamOnline.Entities.Questions;
using QuizExamOnline.Enums;
using QuizExamOnline.Repositories;

namespace QuizExamOnline.Services.Questions
{
    public interface IQuestionService
    {
        Task<QuestionDto> Create(CreateQuestionDto questionDto);
        Task<Paging<QuestionDto>> GetAllQuestions(int page);
        Task<QuestionDto> Update(UpdateQuestionDto questionDto);
        Task<bool> Delete(long id);
        Task<List<QuestionDto>> GetListQuestionsUser();
        Task<List<QuestionDto>> GetQuestionByExam(long id);
        Task<Paging<QuestionDto>> Search(string search, int page);
        Task<List<QuestionDto>> GetAllQuestionsNoPaging();
        Task<List<QuestionDto>> SearchNoPaging(string search);
    }
    public class QuestionService : IQuestionService
    {
        //private readonly IQuestionRepository _questionRepository;
        //private readonly IAnswerQuestionRepository _answerQuestionRepository;
        //private readonly IGeneralRepository _generalRepository;
        private readonly IUnitOfWork _UOW;
        public QuestionService(IUnitOfWork unitOfWork) {
            //_questionRepository = questionRepository;   
            //_answerQuestionRepository = answerQuestionRepository;
            //_generalRepository = generalRepository;
            _UOW = unitOfWork;
        }

        public async Task<QuestionDto> Create(CreateQuestionDto createQuestionDto)
        {
            await ValidateQuestion(createQuestionDto);
            var result = await _UOW.QuestionRepository.Create(createQuestionDto);
            result.Answers = await _UOW.AnswerQuestionRepository.BulkInsert(createQuestionDto.CreateAnswerQuestionDtos, result.Id);
            return result;
        }

        public async Task<QuestionDto> Update(UpdateQuestionDto updateQuestionDto)
        {
            CreateQuestionDto createQuestion = new CreateQuestionDto
            {
                GradeId = updateQuestionDto.GradeId,
                LevelId = updateQuestionDto.LevelId,    
                StatusId = updateQuestionDto.StatusId,  
                SubjectId = updateQuestionDto.SubjectId,
                QuestionGroupId = updateQuestionDto.QuestionGroupId,
                QuestionTypeId = updateQuestionDto.QuestionTypeId,
                CreateAnswerQuestionDtos = updateQuestionDto.CreateAnswerQuestionDtos,
            };

            await ValidateQuestion(createQuestion);
            var result = await _UOW.QuestionRepository.Update(updateQuestionDto);
            if (result == null) throw new CustomException(QuestionErrorEnum.QuestionDoesNotExist);

            await _UOW.AnswerQuestionRepository.Delete(result.Id);
            result.Answers = await _UOW.AnswerQuestionRepository.BulkInsert(updateQuestionDto.CreateAnswerQuestionDtos, result.Id);
            return result;
        }

        public async Task<bool> Delete(long id)
        {
            var question = await _UOW.QuestionRepository.GetQuestionById(id);
            if (question == null) throw new CustomException(QuestionErrorEnum.CanNotChange);
            await _UOW.AnswerQuestionRepository.Delete(id);
            return await _UOW.QuestionRepository.Delete(id);
        }

        public async Task<List<QuestionDto>> GetQuestionByExam(long id)
        {
            var result = await _UOW.QuestionRepository.GetQuestionByExam(id);
            if (result == null) return null;
            foreach(var item in result)
            {
                item.Answers = await _UOW.AnswerQuestionRepository.getListByQuestion(item.Id);
            }
            return result;
        }

        public async Task<List<QuestionDto>> GetListQuestionsUser()
        {
            var result = await _UOW.QuestionRepository.GetListQuestionsUser();
            foreach (var item in result)
            {
                item.Answers = await _UOW.AnswerQuestionRepository.getListByQuestion(item.Id);
            }
            return result;
        }

        public async Task<Paging<QuestionDto>> Search(string search, int page)
        {
            string filter = search.Trim();
            var result = await _UOW.QuestionRepository.Search(filter, page);
            //if (page > result.TotalPage) throw new CustomException(ExamErrorEnum.InvalidPage);
            if (result.Data.Count > 0)
            {
                foreach (var item in result.Data)
                {
                    item.Answers = await _UOW.AnswerQuestionRepository.getListByQuestion(item.Id);
                }
            }
            return result;
        }

        public async Task<List<QuestionDto>> SearchNoPaging(string search)
        {
            string filter = search.Trim();
            var result = await _UOW.QuestionRepository.SearchNoPaging(filter);
            //if (page > result.TotalPage) throw new CustomException(ExamErrorEnum.InvalidPage);
            foreach (var item in result)
            {
                item.Answers = await _UOW.AnswerQuestionRepository.getListByQuestion(item.Id);
            }
            return result;
        }

        public async Task<Paging<QuestionDto>> GetAllQuestions(int page)
        {
            var result = await _UOW.QuestionRepository.GetAllQuestions(page);
            foreach (var item in result.Data)
            {
                item.Answers = await _UOW.AnswerQuestionRepository.getListByQuestion(item.Id);
            }
            return result;
        }

        public async Task<List<QuestionDto>> GetAllQuestionsNoPaging()
        {
            var result = await _UOW.QuestionRepository.GetAllQuestionsNoPaging();
            foreach (var item in result)
            {
                item.Answers = await _UOW.AnswerQuestionRepository.getListByQuestion(item.Id);
            }
            return result;
        }

        private bool CheckAnswer(List<CreateAnswerQuestionDto> createAnswerQuestionDtos, long id)
        {
            int count = 0;
            foreach(var item in createAnswerQuestionDtos)
            {
                if (string.IsNullOrWhiteSpace(item.Content)) return false;
                if (item.IsRight == true) count++;  
            }
            if (count == 0) return false;
            if (id == 1 && count > 1) return false;
            if (id == 2 && count < 2) return false;
            return true;
        }

        private async Task ValidateQuestion(CreateQuestionDto createQuestionDto)
        {
            if (!await _UOW.GeneralRepository.CheckGrade(createQuestionDto.GradeId))
            {
                throw new CustomException(GeneralEnum.GradeDoesNotExist);
            }
            if (!await _UOW.GeneralRepository.CheckLevel(createQuestionDto.LevelId))
            {
                throw new CustomException(GeneralEnum.LevelDoesNotExist);
            }
            if (!await _UOW.GeneralRepository.CheckStatus(createQuestionDto.StatusId))
            {
                throw new CustomException(GeneralEnum.StatusDoesNotExist);
            }
            if (!await _UOW.GeneralRepository.CheckSubject(createQuestionDto.GradeId))
            {
                throw new CustomException(GeneralEnum.SubjectDoesNotExist);
            }
            if (!await _UOW.GeneralRepository.CheckQuestionGroup(createQuestionDto.GradeId))
            {
                throw new CustomException(GeneralEnum.QuestionGroupDoesNotExist);
            }
            if (!await _UOW.GeneralRepository.CheckQuestionType(createQuestionDto.GradeId))
            {
                throw new CustomException(GeneralEnum.QuestionTypeNotExist);
            }
            if (string.IsNullOrWhiteSpace(createQuestionDto.Content))
            {
                throw new CustomException(QuestionErrorEnum.ContentEmpty);
            }
            if (createQuestionDto.CreateAnswerQuestionDtos.Count() == 0)
            {
                throw new CustomException(QuestionErrorEnum.AnswerEmpty);
            }
            if (!CheckAnswer(createQuestionDto.CreateAnswerQuestionDtos, createQuestionDto.QuestionTypeId))
            {
                throw new CustomException(QuestionErrorEnum.InvalidAnswer);
            }
        }
    }
}
