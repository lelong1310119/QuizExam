//using QuizExamOnline.Models;

//namespace QuizExamOnline.Repositories
//{
//    public interface IUnitOfWork : IServiceScope, IDisposable
//    {
//        Task Begin();
//        Task Commit();
//        Task Rollback();

//        IAppUserRepository AppUserRepository { get; }
//        IGeneralRepository GeneralRepository { get; }
//        IQuestionRepository QuestionRepository { get; }
//        IAnswerQuestionRepository AnswerQuestionRepository { get; }
//        IExamRepository ExamRepository { get; }
//        IHistoryExamRepository HistoryExamRepository { get; }
        
//    }

//    public class UnitOfWork : IUnitOfWork
//    {
//        private DataContext DataContext;

//        public IAppUserRepository AppUserRepository { get; private set; }
//        public IGeneralRepository GeneralRepository { get; private set; }
//        public IQuestionRepository QuestionRepository { get; private set; }
//        public IAnswerQuestionRepository AnswerQuestionRepository { get; private set; }
//        public IExamRepository ExamRepository { get; private set; }
//        public IHistoryExamRepository HistoryExamRepository { get; private set; }

//        public UnitOfWork(DataContext DataContext, IRedisS RedisStore, IConfiguration Configuration)
//        {
//            this.DataContext = DataContext;
//            AppUserRepository = new AppUserRepository(DataContext)
            
//        }
//        public async Task Begin()
//        {
//            return;
//            await DataContext.Database.BeginTransactionAsync();
//        }

//        public Task Commit()
//        {
//            //DataContext.Database.CommitTransaction();
//            return Task.CompletedTask;
//        }

//        public Task Rollback()
//        {
//            //DataContext.Database.RollbackTransaction();
//            return Task.CompletedTask;
//        }

//        public void Dispose()
//        {
//            this.Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposing)
//            {
//                return;
//            }

//            if (this.DataContext == null)
//            {
//                return;
//            }

//            this.DataContext.Dispose();
//            this.DataContext = null;
//        }
//    }
//}
