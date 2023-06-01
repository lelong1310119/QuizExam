using Nest;
using QuizExamOnline.Entities.Exams;
using QuizExamOnline.Entities.Questions;
using QuizExamOnline.Repositories;
using QuizExamOnline.Services.Questions;

namespace QuizExamOnline.Services.Exams
{
    public interface IExamService
    {
        Task<ExamDto> CreateExam(CreateExamDto createExamDto);
        Task<ExamDto> Update(UpdateExamDto updateExamDto);
        Task<bool> Delete(long id);
        Task<List<ExamDto>> GetAllExam();
        Task<List<ExamDto>> GetExamByUser();
        Task<List<ExamDto>> Search(string filter);
        Task<ExamDto> GetExamByCode(string code);
        Task<ExamDto> GetExamById(long id);
    }
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IHistoryExamRepository _historyExamRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerQuestionRepository _answerQuestionRepository;
        private readonly IGeneralRepository _generalRepository;

        public ExamService(IExamRepository examRepository,  IHistoryExamRepository historyExamRepository, IQuestionRepository questionRepository, IAnswerQuestionRepository answerQuestionRepository, IGeneralRepository generalRepository)
        {
            _examRepository = examRepository;
            _historyExamRepository = historyExamRepository;
            _questionRepository = questionRepository;
            _answerQuestionRepository = answerQuestionRepository;
            _generalRepository = generalRepository;
        }

        public async Task<ExamDto> CreateExam(CreateExamDto createExamDto)
        {
            await ValidateExam(createExamDto);
            var result = await _examRepository.CreateExam(createExamDto);
            result.Questions = await _questionRepository.GetQuestionByExam(result.Id);
            result.TimeExam = await _historyExamRepository.CountByExam(result.Id);
            foreach (var item in result.Questions)
            {
                item.Answers = await _answerQuestionRepository.getListByQuestion(item.Id);
            }
            return result;
        }

        public async Task<ExamDto> Update(UpdateExamDto updateExamDto)
        {
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
            var result = await _examRepository.Update(updateExamDto);
            if (result == null) throw new ExamException("Exam does not exist");
            result.Questions = await _questionRepository.GetQuestionByExam(result.Id);
            result.TimeExam = await _historyExamRepository.CountByExam(result.Id);
            foreach (var item in  result.Questions)
            {
                item.Answers = await _answerQuestionRepository.getListByQuestion(item.Id);
            }
            return result;
        }
        public async Task<bool> Delete(long id)
        {
            var result = await _examRepository.GetExamById(id);
            if (result != null && result.StatusId != 3) throw new ExamException("Can not be changed");
            return await _examRepository.Delete(id);
        }
        public async Task<List<ExamDto>> GetAllExam()
        {
            var result = await _examRepository.GetAllExam();
            foreach(var item in result)
            {
                item.Questions = await _questionRepository.GetQuestionByExam(item.Id);
                item.TimeExam = await _historyExamRepository.CountByExam(item.Id);
                foreach (var item1 in item.Questions)
                {
                    item1.Answers = await _answerQuestionRepository.getListByQuestion(item1.Id);
                }
            }
            return result;
        }

        public async Task<List<ExamDto>> GetExamByUser()
        {
            var result =  await _examRepository.GetExamByUser();
            foreach (var item in result)
            {
                item.Questions = await _questionRepository.GetQuestionByExam(item.Id);
                item.TimeExam = await _historyExamRepository.CountByExam(item.Id);
                foreach (var item1 in item.Questions)
                {
                    item1.Answers = await _answerQuestionRepository.getListByQuestion(item1.Id);
                }
            }
            return result;
        }
        public async Task<List<ExamDto>> Search(string filter)
        {
            var result = await _examRepository.Search(filter);
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.Questions = await _questionRepository.GetQuestionByExam(item.Id);
                    item.TimeExam = await _historyExamRepository.CountByExam(item.Id);
                    foreach (var item1 in item.Questions)
                    {
                        item1.Answers = await _answerQuestionRepository.getListByQuestion(item1.Id);
                    }
                }
            }
            return result;
        }

        public async Task<ExamDto> GetExamByCode(string code)
        {
            var result = await _examRepository.GetExamByCode(code);
            if (result == null) throw new ExamException("Not found exam");
            result.Questions = await _questionRepository.GetQuestionByExam(result.Id);
            result.TimeExam = await _historyExamRepository.CountByExam(result.Id);
            foreach (var item in result.Questions)
            {
                item.Answers = await _answerQuestionRepository.getListByQuestion(item.Id);
            }
            return result;
        }

        public async Task<ExamDto> GetExamById(long id)
        {
            var result = await _examRepository.GetExamById(id);
            if (result == null) throw new ExamException("Not found exam");
            result.Questions = await _questionRepository.GetQuestionByExam(result.Id);
            result.TimeExam = await _historyExamRepository.CountByExam(result.Id);
            foreach (var item in result.Questions)
            {
                item.Answers = await _answerQuestionRepository.getListByQuestion(item.Id);
            }
            return result;
        }

        private async Task ValidateExam(CreateExamDto createExamDto)
        {
            if (!await _generalRepository.CheckGrade(createExamDto.GradeId))
            {
                throw new ExamException("Grade does not exist");
            }
            if (!await _generalRepository.CheckLevel(createExamDto.LevelId))
            {
                throw new ExamException("Level does not exist");
            }
            if (!await _generalRepository.CheckStatus(createExamDto.StatusId))
            {
                throw new ExamException("Status does not exist");
            }
            if (!await _generalRepository.CheckSubject(createExamDto.GradeId))
            {
                throw new ExamException("Subject does not exist");
            }
            if (string.IsNullOrWhiteSpace(createExamDto.Name))
            {
                throw new ExamException("Empty Name");
            }
            if (string.IsNullOrWhiteSpace(createExamDto.Code))
            {
                throw new ExamException("Empty Code");
            }
            if (createExamDto.IdQuestions.Count() == 0)
            {
                throw new ExamException("Empty Question");
            }
            if (createExamDto.Time < 0)
            {
                throw new ExamException("Invalid Time");
            }
            foreach (var item in createExamDto.IdQuestions)
            {
                if (!await _questionRepository.CheckQuestion(item)) new ExamException("Question does not exist");
            }
        }
    }
}
