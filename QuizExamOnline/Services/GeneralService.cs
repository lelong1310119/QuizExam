using QuizExamOnline.Entities;
using QuizExamOnline.Repositories;

namespace QuizExamOnline.Services
{
    public interface IGeneralService
    {
        Task<List<EntityEnumDto>> getListGrade();
        Task<List<EntityEnumDto>> getListLevel();
        Task<List<EntityEnumDto>> getListStatus();
        Task<List<EntityEnumDto>> getListQuestionGroup();
        Task<List<EntityEnumDto>> getListQuestionType();
        Task<List<EntityEnumDto>> getListSubject();
    }
    public class GeneralService : IGeneralService
    {
        //private readonly IGeneralRepository _generalRepository;
        private readonly IUnitOfWork _UOW;
        public GeneralService(IUnitOfWork unitOfWork)
        {
            //_generalRepository = generalRepository;
            _UOW = unitOfWork;
        }

        public async Task<List<EntityEnumDto>> getListGrade()
        {
            return await _UOW.GeneralRepository.getListGrade();
        }
        public async Task<List<EntityEnumDto>> getListLevel()
        {
            return await _UOW.GeneralRepository.getListLevel();
        }
        public async Task<List<EntityEnumDto>> getListStatus()
        {
            return await _UOW.GeneralRepository.getListStatus();
        }
        public async Task<List<EntityEnumDto>> getListQuestionGroup()
        {
            return await _UOW.GeneralRepository.getListQuestionGroup();
        }
        public async Task<List<EntityEnumDto>> getListQuestionType()
        {
            return await _UOW.GeneralRepository.getListQuestionType();
        }
        public async Task<List<EntityEnumDto>> getListSubject()
        {
            return await _UOW.GeneralRepository.getListSubject();
        }
    }
}
