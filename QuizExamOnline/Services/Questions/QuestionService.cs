using Microsoft.CodeAnalysis.CSharp.Syntax;
using Nest;
using QuizExamOnline.Entities.AnswerQuestions;
using QuizExamOnline.Entities.Questions;
using QuizExamOnline.Repositories;

namespace QuizExamOnline.Services.Questions
{
    public interface IQuestionService
    {
        Task<QuestionDto> Create(CreateQuestionDto questionDto);
        Task<List<QuestionDto>> GetAllQuestions();
        Task<QuestionDto> Update(UpdateQuestionDto questionDto);
        Task<bool> Delete(long id);
        Task<List<QuestionDto>> GetListQuestionsUser();
        Task<List<QuestionDto>> GetQuestionByExam(long id);
    }
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerQuestionRepository _answerQuestionRepository;
        private readonly IGeneralRepository _generalRepository;
        public QuestionService(IQuestionRepository questionRepository, IAnswerQuestionRepository answerQuestionRepository, IGeneralRepository generalRepository) {
            _questionRepository = questionRepository;   
            _answerQuestionRepository = answerQuestionRepository;
            _generalRepository = generalRepository;
        }

        public async Task<QuestionDto> Create(CreateQuestionDto createQuestionDto)
        {
            await ValidateQuestion(createQuestionDto);
            var result = await _questionRepository.Create(createQuestionDto);
            result.Answers = await _answerQuestionRepository.BulkInsert(createQuestionDto.CreateAnswerQuestionDtos, result.Id);
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
            var result = await _questionRepository.Update(updateQuestionDto);
            if (result == null) throw new QuestionException("Question does not exist");

            await _answerQuestionRepository.Delete(result.Id);
            result.Answers = await _answerQuestionRepository.BulkInsert(updateQuestionDto.CreateAnswerQuestionDtos, result.Id);
            return result;
        }

        public async Task<bool> Delete(long id)
        {
            var question = await _questionRepository.GetQuestionById(id);
            if (question == null) throw new QuestionException("Can be not change");
            await _answerQuestionRepository.Delete(id);
            return await _questionRepository.Delete(id);
        }

        public async Task<List<QuestionDto>> GetQuestionByExam(long id)
        {
            var result = await _questionRepository.GetQuestionByExam(id);
            if (result == null) return null;
            foreach(var item in result)
            {
                item.Answers = await _answerQuestionRepository.getListByQuestion(item.Id);
            }
            return result;
        }

        public async Task<List<QuestionDto>> GetListQuestionsUser()
        {
            var result = await _questionRepository.GetListQuestionsUser();
            foreach (var item in result)
            {
                item.Answers = await _answerQuestionRepository.getListByQuestion(item.Id);
            }
            return result;
        }

        public async Task<List<QuestionDto>> GetAllQuestions()
        {
            var result = await _questionRepository.GetAllQuestions();
            foreach (var item in result)
            {
                item.Answers = await _answerQuestionRepository.getListByQuestion(item.Id);
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
            if (!await _generalRepository.CheckGrade(createQuestionDto.GradeId))
            {
                throw new QuestionException("Grade does not exist");
            }
            if (!await _generalRepository.CheckLevel(createQuestionDto.LevelId))
            {
                throw new QuestionException("Level does not exist");
            }
            if (!await _generalRepository.CheckStatus(createQuestionDto.StatusId))
            {
                throw new QuestionException("Status does not exist");
            }
            if (!await _generalRepository.CheckSubject(createQuestionDto.GradeId))
            {
                throw new QuestionException("Subject does not exist");
            }
            if (!await _generalRepository.CheckQuestionGroup(createQuestionDto.GradeId))
            {
                throw new QuestionException("QuestionGroup does not exist");
            }
            if (!await _generalRepository.CheckQuestionType(createQuestionDto.GradeId))
            {
                throw new QuestionException("QuestionType does not exist");
            }
            if (string.IsNullOrWhiteSpace(createQuestionDto.Content))
            {
                throw new QuestionException("Empty content");
            }
            if (createQuestionDto.CreateAnswerQuestionDtos.Count() == 0)
            {
                throw new QuestionException("Empty Answer");
            }
            if (!CheckAnswer(createQuestionDto.CreateAnswerQuestionDtos, createQuestionDto.QuestionTypeId))
            {
                throw new QuestionException("Invalid Answers");
            }
        }
    }
}
