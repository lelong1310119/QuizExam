using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizExamOnline.Entities;
using QuizExamOnline.Models;

namespace QuizExamOnline.Repositories
{
    public interface IGeneralRepository
    {
        Task<List<EntityEnumDto>> getListGrade();
        Task<List<EntityEnumDto>> getListLevel();
        Task<List<EntityEnumDto>> getListStatus();
        Task<List<EntityEnumDto>> getListQuestionGroup();
        Task<List<EntityEnumDto>> getListQuestionType();
        Task<List<EntityEnumDto>> getListSubject();
        Task<bool> CheckGrade(long id);
        Task<bool> CheckLevel(long id);
        Task<bool> CheckStatus(long id);
        Task<bool> CheckQuestionType(long id);
        Task<bool> CheckQuestionGroup(long id);
        Task<bool> CheckSubject(long id);
    }
    public class GeneralRepository : IGeneralRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;   
        public GeneralRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<List<EntityEnumDto>> getListGrade()
        {
            var result = await _dataContext.Grades.ToListAsync();
            return _mapper.Map<List<Grade>,List<EntityEnumDto>>(result);
        }

        public async Task<List<EntityEnumDto>> getListLevel()
        {
            var result = await _dataContext.Levels.ToListAsync();
            return _mapper.Map<List<Level>,List<EntityEnumDto>>(result);
        }
        public async Task<List<EntityEnumDto>> getListStatus()
        {
            var result = await _dataContext.Statuses.ToListAsync();
            return _mapper.Map<List<Status>,List<EntityEnumDto>>(result);
        }
        public async Task<List<EntityEnumDto>> getListQuestionGroup()
        {
            var result = await _dataContext.QuestionGroups.ToListAsync();
            return _mapper.Map<List<QuestionGroup>,List<EntityEnumDto>>(result);
        }
        public async Task<List<EntityEnumDto>> getListQuestionType()
        {
            var result = await _dataContext.QuestionTypes.ToListAsync();
            return _mapper.Map<List<QuestionType>, List<EntityEnumDto>>(result);
        }

        public async Task<List<EntityEnumDto>> getListSubject()
        {
            var result = await _dataContext.Subjects.ToListAsync();
            return _mapper.Map<List<Subject>,List<EntityEnumDto>>(result);
        }

        public async Task<bool> CheckGrade(long id)
        {
            var result = await _dataContext.Grades
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckLevel(long id)
        {
            var result = await _dataContext.Levels
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckStatus(long id)
        {
            var result = await _dataContext.Statuses
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckQuestionType(long id)
        {
            var result = await _dataContext.QuestionTypes
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckQuestionGroup(long id)
        {
            var result = await _dataContext.QuestionGroups
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckSubject(long id)
        {
            var result = await _dataContext.Subjects
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();
            if (result == null)
            {
                return false;
            }
            return true;
        }

    }
}
