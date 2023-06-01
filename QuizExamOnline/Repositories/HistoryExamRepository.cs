using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nest;
using QuizExamOnline.Common;
using QuizExamOnline.Entities.Histories;
using QuizExamOnline.Models;

namespace QuizExamOnline.Repositories
{
    public interface IHistoryExamRepository {
        Task<List<ExamHistoryDto>> GetHistoryByExam(long id);
        Task<List<MyHistoryDto>> GetMyHistory();
        Task<MyHistoryDto> Create(CreateHistoryExamDto createExamDto);
        Task<int> CountByExam(long id);
    }

    public class HistoryExamRepository : IHistoryExamRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ICurrentContext _currentContext;

        public HistoryExamRepository(DataContext dataContext, IMapper mapper, ICurrentContext currentContext)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _currentContext = currentContext;
        }

        public async Task<List<ExamHistoryDto>> GetHistoryByExam(long id)
        {
            var result = await _dataContext.ExamHistories
                                    .Where(x => x.ExamId == id)
                                    .ToListAsync();
            return _mapper.Map<List<ExamHistory>, List<ExamHistoryDto>>(result);
        }

        public async Task<List<MyHistoryDto>> GetMyHistory()
        {
            var result = await _dataContext.ExamHistories
                                    .Where(x => x.AppUserId == _currentContext.UserId)
                                    .ToListAsync();
            return _mapper.Map<List<ExamHistory>, List<MyHistoryDto>>(result);
        }

        public async Task<MyHistoryDto> Create(CreateHistoryExamDto createExamDto)
        {
            var history = _mapper.Map<CreateHistoryExamDto, ExamHistory>(createExamDto);
            history.CreateAt = DateTime.Now;
            history.AppUserId = _currentContext.UserId;
            await _dataContext.ExamHistories.AddAsync(history);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<ExamHistory, MyHistoryDto>(history);
        }

        public async Task<int> CountByExam(long id)
        {
            var result = await _dataContext.ExamHistories
                            .Where(x => x.ExamId == id)
                            .CountAsync();
            return result;
        }
    }
}
