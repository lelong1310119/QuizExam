using AutoMapper;
using QuizExamOnline.Common;
using QuizExamOnline.Models;

namespace QuizExamOnline.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();

        IAppUserRepository AppUserRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        IExamRepository ExamRepository { get; }
        IAnswerQuestionRepository AnswerQuestionRepository { get; }
        IHistoryExamRepository HistoryExamRepository { get; }
        IGeneralRepository GeneralRepository { get; }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public IAppUserRepository AppUserRepository { get; private set; }
        public IQuestionRepository QuestionRepository { get; private set; }
        public IExamRepository ExamRepository { get; private set; }
        public IAnswerQuestionRepository AnswerQuestionRepository { get; private set; }
        public IHistoryExamRepository HistoryExamRepository { get; private set; }
        public IGeneralRepository GeneralRepository { get; private set; }
        public UnitOfWork(DataContext dataContext, IMapper mapper, ICurrentContext currentContext) {
            _context = dataContext;
            AppUserRepository = new AppUserRepository(dataContext, mapper, currentContext);
            QuestionRepository = new QuestionRepository(dataContext, mapper, currentContext);
            ExamRepository = new ExamRepository(dataContext, mapper, currentContext);
            AnswerQuestionRepository = new AnswerQuestionRepository(dataContext, mapper);
            HistoryExamRepository = new HistoryExamRepository(dataContext, mapper, currentContext);
            GeneralRepository = new GeneralRepository(dataContext, mapper);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
