using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizExamOnline.Common;
using QuizExamOnline.Entities;
using QuizExamOnline.Entities.AnswerQuestions;
using QuizExamOnline.Entities.Exams;
using QuizExamOnline.Entities.Questions;
using QuizExamOnline.Models;

namespace QuizExamOnline.Repositories
{
    public interface IQuestionRepository {
        Task<QuestionDto> Create(CreateQuestionDto questionDto);
        Task<Paging<QuestionDto>> GetAllQuestions(int page);
        Task<QuestionDto> Update(UpdateQuestionDto questionDto);
        Task<bool> Delete(long id);
        Task<List<QuestionDto>> GetListQuestionsUser();
        Task<List<QuestionDto>> GetQuestionByExam(long id);
        Task<bool> CheckQuestion(long id);
        Task<QuestionDto> GetQuestionById(long id);
        Task<Paging<QuestionDto>> Search(string filter, int page);
        Task<List<QuestionDto>> GetAllQuestionsNoPaging();
        Task<List<QuestionDto>> SearchNoPaging(string filter);

    }
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ICurrentContext _currentContext;

        public QuestionRepository(DataContext dataContext, IMapper mapper, ICurrentContext currentContext)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _currentContext = currentContext;
        }

        public async Task<QuestionDto> Create(CreateQuestionDto questionDto)
        {
            var question = _mapper.Map<CreateQuestionDto, Question>(questionDto);
            question.CreatedAt = DateTime.Now;
            question.UpdatedAt = DateTime.Now;
            question.AppUserId = _currentContext.UserId;

            await _dataContext.Questions.AddAsync(question);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Question, QuestionDto>(question);
        }

        public async Task<List<QuestionDto>> GetListQuestionsUser()
        {
            var result = await _dataContext.Questions
                            .Where(x => x.AppUserId == _currentContext.UserId)
                            .ToListAsync();
            var questions = _mapper.Map<List<Question>, List<QuestionDto>>(result);
            return questions;
        }

        public async Task<List<QuestionDto>> GetAllQuestionsNoPaging()
        {
            var result = await _dataContext.Questions
                            .Where(x => (x.StatusId == 1) || (x.StatusId == 2 && x.AppUserId == _currentContext.UserId))
                            .ToListAsync();
            var questions = _mapper.Map<List<Question>, List<QuestionDto>>(result);
            return questions;
        }

        public async Task<Paging<QuestionDto>> GetAllQuestions(int page)
        {
            var count = await _dataContext.Questions
                            .Where(x => (x.StatusId == 1) || (x.StatusId == 2 && x.AppUserId == _currentContext.UserId))
                            .CountAsync();
            var result = await _dataContext.Questions
                            .Where(x => (x.StatusId == 1) || (x.StatusId == 2 && x.AppUserId == _currentContext.UserId))
                            .Skip((page - 1) * 10)
                            .Take(10)
                            .ToListAsync();
            int totalPage = (count % 10 == 0) ? (count / 10) : (count / 10 + 1);
            var questions = _mapper.Map<List<Question>, List<QuestionDto>>(result);
            //foreach (var question in questions)
            //{
            //    var answer = await _dataContext.AnswerQuestions
            //                            .Where(x => x.QuestionId == question.Id)
            //                            .ToListAsync();
            //    question.Answers = _mapper.Map<List<AnswerQuestion>, List<AnswerQuestionDto>>(answer);
            //}
            Paging<QuestionDto> paging = new Paging<QuestionDto>
            {
                TotalPage = totalPage,
                Data = questions
            };
            return paging;
        }

        public async Task<Paging<QuestionDto>> Search(string filter, int page)
        {
            int count = await _dataContext.Questions
                                .Where(x => (x.Content.Contains(filter)) && ((x.StatusId == 1) || (x.StatusId == 2 && x.AppUserId == _currentContext.UserId)))
                                .CountAsync();
            var result = await _dataContext.Questions
                                    .Where(x => (x.Content.Contains(filter)) && ((x.StatusId == 1) || (x.StatusId == 2 && x.AppUserId == _currentContext.UserId)))
                                    .Skip((page - 1) * 10)
                                    .Take(10)
                                    .ToListAsync();
            int totalPage = (count % 10 == 0) ? (count / 10) : (count / 10 + 1);
            var questions = _mapper.Map<List<Question>, List<QuestionDto>>(result);
            //foreach (var question in questions)
            //{
            //    var answer = await _dataContext.AnswerQuestions
            //                            .Where(x => x.QuestionId == question.Id)
            //                            .ToListAsync();
            //    question.Answers = _mapper.Map<List<AnswerQuestion>, List<AnswerQuestionDto>>(answer);
            //}
            Paging<QuestionDto> paging = new Paging<QuestionDto>
            {
                TotalPage = totalPage,
                Data = questions
            };
            return paging;
        }

        public async Task<List<QuestionDto>> SearchNoPaging(string filter)
        {
            var result = await _dataContext.Questions
                                    .Where(x => (x.Content.Contains(filter)) && ((x.StatusId == 1) || (x.StatusId == 2 && x.AppUserId == _currentContext.UserId)))
                                    .ToListAsync();
            var questions = _mapper.Map<List<Question>, List<QuestionDto>>(result);
            //foreach (var question in questions)
            //{
            //    var answer = await _dataContext.AnswerQuestions
            //                            .Where(x => x.QuestionId == question.Id)
            //                            .ToListAsync();
            //    question.Answers = _mapper.Map<List<AnswerQuestion>, List<AnswerQuestionDto>>(answer);
            //}
            return questions;
        }

        public async Task<QuestionDto> GetQuestionById(long id)
        {
            var result = await _dataContext.Questions
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();
            if (result == null) return null;
            return _mapper.Map<Question, QuestionDto>(result);
        }

        public async Task<List<QuestionDto>> GetQuestionByExam(long id)
        {
            var result = await _dataContext.ExamQuestions
                                .Where(x => x.ExamId == id)
                                .ToListAsync();
            if (result.Count == 0) return null;
            
            List<QuestionDto> questions = new List<QuestionDto> ();
            foreach(var item in  result)
            {
                var questionModel = await _dataContext.Questions
                                        .Where(x => x.Id == item.QuestionId)
                                        .FirstOrDefaultAsync();
                var question = _mapper.Map<Question, QuestionDto>(questionModel);
                var answer = await _dataContext.AnswerQuestions
                                        .Where(x => x.QuestionId == question.Id)
                                        .ToListAsync();
                question.Answers = _mapper.Map<List<AnswerQuestion>, List<AnswerQuestionDto>>(answer);
                questions.Add(question);
            }
            return questions;
        }

        public async Task<QuestionDto> Update(UpdateQuestionDto questionDto)
        {
            var question = _dataContext.Questions
                                    .Where(x => x.Id == questionDto.Id)
                                    .FirstOrDefault();
            if (question == null) return null;

            question.UpdatedAt = DateTime.Now;
            question.Content = questionDto.Content;
            question.GradeId = questionDto.GradeId;
            question.LevelId = questionDto.LevelId;
            question.SubjectId = questionDto.SubjectId;
            question.QuestionGroupId = questionDto.QuestionGroupId;
            question.QuestionTypeId = questionDto.QuestionTypeId;
            question.StatusId = questionDto.StatusId;

            await _dataContext.SaveChangesAsync();
            return _mapper.Map<Question, QuestionDto>(question);   
        }

        public async Task<bool> Delete(long id)
        {
            var question = await _dataContext.Questions
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();
            if (question == null) return false;
            _dataContext.Questions.Remove(question);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckQuestion(long id)
        {
            var result = await _dataContext.Questions
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();
            if (result == null) return false;
            return true;
        }
        //private async Task SaveReference(List<CreateAnswerQuestionDto> createAnswerQuestionDtos, long id)
        //{
        //    var answers = _mapper.Map <List<AnswerQuestion>>(createAnswerQuestionDtos);
        //    foreach (var answer in answers)
        //    {
        //        answer.QuestionId = id;
        //    }
        //    await _dataContext.AnswerQuestions.BulkInsertAsync(answers);
        //    await _dataContext.SaveChangesAsync();
        //}

        //private async Task DeleteReference(long id)
        //{
        //    var answers = await _dataContext.AnswerQuestions
        //                    .Where(x => x.QuestionId == id)
        //                    .ToListAsync();
        //    await _dataContext.AnswerQuestions.BulkDeleteAsync(answers);
        //    await _dataContext.SaveChangesAsync();
        //}
    }
}
