using Microsoft.Identity.Client;
using Nest;
using QuizExamOnline.Common;
using QuizExamOnline.Entities;
using QuizExamOnline.Entities.Exams;
using QuizExamOnline.Entities.Questions;
using QuizExamOnline.Enums;
using QuizExamOnline.Repositories;
using QuizExamOnline.Services.Questions;

namespace QuizExamOnline.Services.Exams
{
    public interface IExamService
    {
        Task<ExamDto> CreateExam(CreateExamDto createExamDto);
        Task<ExamDto> Update(UpdateExamDto updateExamDto);
        Task<bool> Delete(long id);
        Task<Paging<ExamDto>> GetAllExam(int page);
        Task<List<ExamDto>> GetExamByUser();
        Task<Paging<ExamDto>> Search(string filter, int page);
        Task<ExamDto> GetExamByCode(string code);
        Task<ExamDto> GetExamById(long id);
        Task<List<ExamDto>> SearchNoPaging(string search);
        Task<List<ExamDto>> GetAllExamNoPaging();
    }
    public class ExamService : IExamService
    {
        //private readonly IExamRepository _examRepository;
        //private readonly IHistoryExamRepository _historyExamRepository;
        //private readonly IQuestionRepository _questionRepository;
        //private readonly IAnswerQuestionRepository _answerQuestionRepository;
        //private readonly IGeneralRepository _generalRepository;
        private readonly IUnitOfWork _UOW;

        public ExamService(IUnitOfWork UOW)
        {
            //_examRepository = examRepository;
            //_historyExamRepository = historyExamRepository;
            //_questionRepository = questionRepository;
            //_answerQuestionRepository = answerQuestionRepository;
            //_generalRepository = generalRepository;
            _UOW = UOW;
        }

        public async Task<ExamDto> CreateExam(CreateExamDto createExamDto)
        {
            createExamDto.Code = createExamDto.Code.Trim();
            await ValidateExam(createExamDto);
            var result = await _UOW.ExamRepository.CreateExam(createExamDto);
            result.Questions = await _UOW.QuestionRepository.GetQuestionByExam(result.Id);
            result.TimeExam = await _UOW.HistoryExamRepository.CountByExam(result.Id);
            if (result.Questions != null)
            {
                foreach (var item in result.Questions)
                {
                    item.Answers = await _UOW.AnswerQuestionRepository.getListByQuestion(item.Id);
                }
            }
            return result;
        }

        public async Task<ExamDto> Update(UpdateExamDto updateExamDto)
        {
            var exam = await _UOW.ExamRepository.GetExamById(updateExamDto.Id);
            if (exam != null && exam.StatusId != 3) throw new CustomException(ExamErrorEnum.CanNotChange);
            updateExamDto.Code = updateExamDto.Code.Trim();
            CreateExamDto createExamDto = new CreateExamDto
            {
                Code = updateExamDto.Code,
                Name = updateExamDto.Name,
                LevelId = updateExamDto.LevelId,
                StatusId = updateExamDto.StatusId,
                GradeId = updateExamDto.GradeId,
                SubjectId = updateExamDto.SubjectId,
                Time = updateExamDto.Time,
                IdQuestions = updateExamDto.IdQuestions
            };
            await ValidateExam(createExamDto);
            var result = await _UOW.ExamRepository.Update(updateExamDto);
            if (result == null) throw new CustomException(ExamErrorEnum.ExamDoesNotExist);
            result.Questions = await _UOW.QuestionRepository.GetQuestionByExam(result.Id);
            result.TimeExam = await _UOW.HistoryExamRepository.CountByExam(result.Id);
            if (result.Questions != null)
            {
                foreach (var item in result.Questions)
                {
                    item.Answers = await _UOW.AnswerQuestionRepository.getListByQuestion(item.Id);
                }
            }
            return result;
        }

        public async Task<bool> Delete(long id)
        {
            var result = await _UOW.ExamRepository.GetExamById(id);
            if (result != null && result.StatusId != 3) throw new CustomException(ExamErrorEnum.CanNotChange);
            return await _UOW.ExamRepository.Delete(id);
        }
        public async Task<Paging<ExamDto>> GetAllExam(int page)
        {
            var result = await _UOW.ExamRepository.GetAllExam(page);
            //if (page > result.TotalPage) throw new CustomException(ExamErrorEnum.InvalidPage);
            foreach (var item in result.Data)
            {
                item.Questions = await _UOW.QuestionRepository.GetQuestionByExam(item.Id);
                item.TimeExam = await _UOW.HistoryExamRepository.CountByExam(item.Id);
                if (item.Questions != null)
                {
                    foreach (var item1 in item.Questions)
                    {
                        item1.Answers = await _UOW.AnswerQuestionRepository.getListByQuestion(item1.Id);
                    }
                }
            }
            return result;
        }

        public async Task<List<ExamDto>> GetAllExamNoPaging()
        {
            var result = await _UOW.ExamRepository.GetAllExamNoPaging();
            //if (page > result.TotalPage) throw new CustomException(ExamErrorEnum.InvalidPage);
            foreach (var item in result)
            {
                item.Questions = await _UOW.QuestionRepository.GetQuestionByExam(item.Id);
                item.TimeExam = await _UOW.HistoryExamRepository.CountByExam(item.Id);
                if (item.Questions != null)
                {
                    foreach (var item1 in item.Questions)
                    {
                        item1.Answers = await _UOW.AnswerQuestionRepository.getListByQuestion(item1.Id);
                    }
                }
            }
            return result;
        }

        public async Task<List<ExamDto>> GetExamByUser()
        {
            var result =  await _UOW.ExamRepository.GetExamByUser();
            foreach (var item in result)
            {
                item.Questions = await _UOW.QuestionRepository.GetQuestionByExam(item.Id);
                item.TimeExam = await _UOW.HistoryExamRepository.CountByExam(item.Id);
                if (item.Questions != null)
                {
                    foreach (var item1 in item.Questions)
                    {
                        item1.Answers = await _UOW.AnswerQuestionRepository.getListByQuestion(item1.Id);
                    }
                }
            }
            return result;
        }

        public async Task<Paging<ExamDto>> Search(string search, int page)
        {
            string filter = search.Trim();
            var result = await _UOW.ExamRepository.Search(filter, page);
            //if (page > result.TotalPage) throw new CustomException(ExamErrorEnum.InvalidPage);
            if (result.Data.Count > 0)
            {
                foreach (var item in result.Data)
                {
                    item.Questions = await _UOW.QuestionRepository.GetQuestionByExam(item.Id);
                    item.TimeExam = await _UOW.HistoryExamRepository.CountByExam(item.Id);
                    if (item.Questions != null)
                    {
                        foreach (var item1 in item.Questions)
                        {
                            item1.Answers = await _UOW.AnswerQuestionRepository.getListByQuestion(item1.Id);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<ExamDto>> SearchNoPaging(string search)
        {
            string filter = search.Trim();
            var result = await _UOW.ExamRepository.SearchNoPaging(filter);
            //if (page > result.TotalPage) throw new CustomException(ExamErrorEnum.InvalidPage);
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.Questions = await _UOW.QuestionRepository.GetQuestionByExam(item.Id);
                    item.TimeExam = await _UOW.HistoryExamRepository.CountByExam(item.Id);
                    if (item.Questions != null)
                    {
                        foreach (var item1 in item.Questions)
                        {
                            item1.Answers = await _UOW.AnswerQuestionRepository.getListByQuestion(item1.Id);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<ExamDto> GetExamByCode(string _code)
        {
            string code = _code.Trim();
            var result = await _UOW.ExamRepository.GetExamByCode(code);
            if (result == null) throw new CustomException(ExamErrorEnum.NotFoundExam);
            result.Questions = await _UOW.QuestionRepository.GetQuestionByExam(result.Id);
            result.TimeExam = await _UOW.HistoryExamRepository.CountByExam(result.Id);
            if (result.Questions != null)
            {
                foreach (var item in result.Questions)
                {
                    item.Answers = await _UOW.AnswerQuestionRepository.getListByQuestion(item.Id);
                }
            }
            return result;
        }

        public async Task<ExamDto> GetExamById(long id)
        {
            var result = await _UOW.ExamRepository.GetExamById(id);
            if (result == null) throw new CustomException(ExamErrorEnum.NotFoundExam);
            result.Questions = await _UOW.QuestionRepository.GetQuestionByExam(result.Id);
            result.TimeExam = await _UOW.HistoryExamRepository.CountByExam(result.Id);
            if (result.Questions != null)
            {
                foreach (var item in result.Questions)
                {
                    item.Answers = await _UOW.AnswerQuestionRepository.getListByQuestion(item.Id);
                }
            }
            return result;
        }

        private async Task ValidateExam(CreateExamDto createExamDto)
        {
            if (!await _UOW.GeneralRepository.CheckGrade(createExamDto.GradeId))
            {
                throw new CustomException(GeneralEnum.GradeDoesNotExist);
            }
            if (!await _UOW.GeneralRepository.CheckLevel(createExamDto.LevelId))
            {
                throw new CustomException(GeneralEnum.LevelDoesNotExist);
            }
            if (!await _UOW.GeneralRepository.CheckStatus(createExamDto.StatusId))
            {
                throw new CustomException(GeneralEnum.StatusDoesNotExist);
            }
            if (!await _UOW.GeneralRepository.CheckSubject(createExamDto.GradeId))
            {
                throw new CustomException(GeneralEnum.SubjectDoesNotExist);
            }
            if (createExamDto.Name == null || createExamDto.Code.Trim() == "")
            {
                throw new CustomException(ExamErrorEnum.NameEmpty);
            }
            if (string.IsNullOrWhiteSpace(createExamDto.Code) || createExamDto.Code.Contains(" "))
            {
                throw new CustomException(ExamErrorEnum.InvalidCode);
            }
            if (await _UOW.ExamRepository.GetExamByCode(createExamDto.Code) != null)
            {
                throw new CustomException(ExamErrorEnum.CodeAlreadyExists);
            }
            if (createExamDto.IdQuestions.Count() == 0)
            {
                throw new CustomException(ExamErrorEnum.QuestionEmpty);
            }
            if (createExamDto.Time <= 0)
            {
                throw new CustomException(ExamErrorEnum.InvalidTime);
            }
            foreach (var item in createExamDto.IdQuestions)
            {
                if (!await _UOW.QuestionRepository.CheckQuestion(item)) throw new CustomException(ExamErrorEnum.QuestionDoesNotExits);
            }
        }
    }
}
