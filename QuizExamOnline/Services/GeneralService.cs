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
        private readonly IGeneralRepository _generalRepository;
        public GeneralService(IGeneralRepository generalRepository)
        {
            _generalRepository = generalRepository;
        }

        public async Task<List<EntityEnumDto>> getListGrade()
        {
            return await _generalRepository.getListGrade();
        }
        public async Task<List<EntityEnumDto>> getListLevel()
        {
            return await _generalRepository.getListLevel();
        }
        public async Task<List<EntityEnumDto>> getListStatus()
        {
            return await _generalRepository.getListStatus();
        }
        public async Task<List<EntityEnumDto>> getListQuestionGroup()
        {
            return await _generalRepository.getListQuestionGroup();
        }
        public async Task<List<EntityEnumDto>> getListQuestionType()
        {
            return await _generalRepository.getListQuestionType();
        }
        public async Task<List<EntityEnumDto>> getListSubject()
        {
            return await _generalRepository.getListSubject();
        }
    }
}
