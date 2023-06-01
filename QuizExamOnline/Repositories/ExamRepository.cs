using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nest;
using QuizExamOnline.Common;
using QuizExamOnline.Entities.AnswerQuestions;
using QuizExamOnline.Entities.Exams;
using QuizExamOnline.Entities.Questions;
using QuizExamOnline.Models;

namespace QuizExamOnline.Repositories
{
    public interface IExamRepository
    {
        Task<ExamDto> CreateExam(CreateExamDto createExamDto);
        Task<ExamDto> Update(UpdateExamDto updateExamDto);
        Task<bool> Delete(long id);
        Task<List<ExamDto>> GetAllExam();
        Task<List<ExamDto>> GetExamByUser();
        //Task<List<QuestionDto>> GetQuestionByExam(long id);
        Task<List<ExamDto>> Search(string filter);
        Task<ExamDto> GetExamByCode(string code);
        Task<ExamDto> GetExamById(long id);
        Task<bool> CheckExamQuestion(long idExam, long idQuestion);
    }

    public class ExamRepository : IExamRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ICurrentContext _currentContext;

        public ExamRepository(DataContext dataContext, IMapper mapper, ICurrentContext currentContext)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _currentContext = currentContext;
        }


        public async Task<ExamDto> CreateExam(CreateExamDto createExamDto)
        {
            var exam = _mapper.Map<CreateExamDto, Exam>(createExamDto);

            exam.CreatedAt = DateTime.Now;
            exam.UpdatedAt = DateTime.Now;
            exam.AppUserId = _currentContext.UserId;
            await _dataContext.Exams.AddAsync(exam);
            await _dataContext.SaveChangesAsync();
            await SaveReference(createExamDto.IdQuestions, exam.Id);
            return _mapper.Map<Exam, ExamDto>(exam);
        }

        public async Task<ExamDto> Update(UpdateExamDto updateExamDto)
        {
            var exam = await _dataContext.Exams
                                    .Where(x => x.Id == updateExamDto.Id)
                                    .FirstOrDefaultAsync();

            if (exam == null) return null;

            exam.UpdatedAt = DateTime.Now;
            exam.LevelId = updateExamDto.LevelId;
            exam.SubjectId = updateExamDto.SubjectId;
            exam.GradeId = updateExamDto.GradeId;
            exam.StatusId = updateExamDto.StatusId;
            exam.Image = updateExamDto.Image;
            exam.Code = updateExamDto.Code;
            exam.Name = updateExamDto.Name;
            exam.Time = updateExamDto.Time;
            _dataContext.Exams.Update(exam);

            await DeleteReference(exam.Id);
            await SaveReference(updateExamDto.IdQuestions, exam.Id);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<Exam, ExamDto>(exam);
        }

        public async Task<bool> Delete(long id)
        {
            var exam = await _dataContext.Exams
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();
            if (exam == null) return false;

            await DeleteReference(id);
            _dataContext.Exams.Remove(exam);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<ExamDto> GetExamById(long id)
        {
            var exam = await _dataContext.Exams
                                .Where(x => x.Id == id)
                                .FirstOrDefaultAsync();
            if (exam == null) return null;
            return _mapper.Map<Exam, ExamDto>(exam);
        }

        public async Task<ExamDto> GetExamByCode(string code)
        {
            var exam = await _dataContext.Exams
                                .Where(x => x.Code == code)
                                .FirstOrDefaultAsync();
            if (exam == null) return null;
            return _mapper.Map<Exam, ExamDto>(exam);
        }

        public async Task<List<ExamDto>> Search(string filter)
        {
            var result = await _dataContext.Exams
                                .Where(x => (x.Name.Contains(filter)) && (x.StatusId == 1))
                                .ToListAsync();
            return _mapper.Map<List<Exam>, List<ExamDto>>(result);
        }

        public async Task<List<ExamDto>> GetAllExam()
        {
            var result = await _dataContext.Exams
                            .Where(x => x.StatusId == 1)
                            .ToListAsync();
            return _mapper.Map<List<Exam>, List<ExamDto>>(result);
        }

        public async Task<List<ExamDto>> GetExamByUser()
        {
            var result = await _dataContext.Exams
                            .Where(x => x.AppUserId == _currentContext.UserId)
                            .ToListAsync();
            var exams = _mapper.Map<List<Exam>, List<ExamDto>>(result);
            return exams;
        }


        public async Task SaveReference(List<long> IdQuestions, long id)
        {
            foreach(var idQuestion in IdQuestions)
            {
                ExamQuestion examQuestion = new ExamQuestion
                {
                    QuestionId = idQuestion,
                    ExamId = id
                };
                await _dataContext.ExamQuestions.AddAsync(examQuestion);
            }
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteReference(long id)
        {
            var result = await _dataContext.ExamQuestions
                        .Where(x => x.ExamId == id)
                        .ToListAsync();
            _dataContext.ExamQuestions.RemoveRange(result);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<bool> CheckExamQuestion(long idExam, long idQuestion)
        {
            var result = await _dataContext.ExamQuestions
                                    .Where(x => x.ExamId == idExam && x.QuestionId == idQuestion)
                                    .FirstOrDefaultAsync();
            if (result == null) return false;
            return true;
        }

        //public async Task<List<QuestionDto>> GetQuestionByExam(long id)
        //{
        //    var result = await _dataContext.ExamQuestions
        //                        .Where(x => x.ExamId == id)
        //                        .ToListAsync();

        //    List<QuestionDto> questions = new List<QuestionDto>();
        //    foreach (var item in result)
        //    {
        //        var questionModel = await _dataContext.Questions
        //                                .Where(x => x.Id == item.QuestionId)
        //                                .FirstOrDefaultAsync();
        //        var question = _mapper.Map<QuestionDto>(questionModel);
        //        var answer = await _dataContext.AnswerQuestions
        //                                .Where(x => x.QuestionId == question.Id)
        //                                .ToListAsync();
        //        question.Answers = _mapper.Map<List<AnswerQuestionDto>>(answer);
        //        questions.Add(question);
        //    }
        //    return questions;
        //}
    }
}
