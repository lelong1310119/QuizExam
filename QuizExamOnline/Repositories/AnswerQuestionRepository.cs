using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nest;
using QuizExamOnline.Entities.AnswerQuestions;
using QuizExamOnline.Models;
using Thinktecture;

namespace QuizExamOnline.Repositories
{
    public interface IAnswerQuestionRepository
    {
        Task<List<AnswerQuestionDto>> getListByQuestion(long id);
        Task<bool> Delete(long id);
        Task<List<AnswerQuestionDto>> BulkInsert(List<CreateAnswerQuestionDto> createAnswerQuestionDtos, long id);
        Task<bool> Check(long id);
        Task<int> CountAnswer(long id);
        Task<int> CountAnswerCorrect(long id);
        Task<bool> CheckAnswerQuestion(long idQuestion, long idAnswer);
    }

    public class AnswerQuestionRepository : IAnswerQuestionRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public AnswerQuestionRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<List<AnswerQuestionDto>> getListByQuestion(long id)
        {
            var result = await _dataContext.AnswerQuestions
                                    .Where(x => x.QuestionId == id)
                                    .ToListAsync();
            return _mapper.Map<List<AnswerQuestion>, List<AnswerQuestionDto>>(result);
        }

        public async Task<List<AnswerQuestionDto>> BulkInsert(List<CreateAnswerQuestionDto> createAnswerQuestionDtos, long id)
        {
            var answers = _mapper.Map<List<CreateAnswerQuestionDto>, List<AnswerQuestion>>(createAnswerQuestionDtos);
            foreach (var answer in answers)
            {
                answer.QuestionId = id;
            }

            await _dataContext.AnswerQuestions.AddRangeAsync(answers);
            await _dataContext.SaveChangesAsync();
            var result = await _dataContext.AnswerQuestions
                                    .Where(x => x.QuestionId == id)
                                    .ToListAsync();
            return _mapper.Map<List<AnswerQuestion>, List<AnswerQuestionDto>>(result);
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                var result = await _dataContext.AnswerQuestions
                                    .Where(x => x.QuestionId == id)
                                    .ToListAsync();
                _dataContext.AnswerQuestions.RemoveRange(result);
                await _dataContext.SaveChangesAsync();
            } catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Check(long id)
        {
            var result = await _dataContext.AnswerQuestions
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();
            if (result.IsRight) return true;
            return false;
        }

        public async Task<int> CountAnswer(long id)
        {
            var result = await _dataContext.AnswerQuestions
                                    .Where(x => x.QuestionId == id)
                                    .CountAsync();
            return result;
        }

        public async Task<int> CountAnswerCorrect(long id)
        {
            var result = await _dataContext.AnswerQuestions
                                    .Where(x => (x.QuestionId == id) && (x.IsRight == true))
                                    .CountAsync();
            return result;
        }

        public async Task<bool> CheckAnswerQuestion(long idQuestion, long idAnswer)
        {
            var result = await _dataContext.AnswerQuestions
                                    .Where(x => (x.QuestionId == idQuestion) && (x.Id == idAnswer))
                                    .FirstOrDefaultAsync();
            if (result == null) return false;
            return true;
        }
    }
}
