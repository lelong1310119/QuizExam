using Nest;
using QuizExamOnline.Common;
using QuizExamOnline.Entities.Histories;
using QuizExamOnline.Enums;
using QuizExamOnline.Repositories;

namespace QuizExamOnline.Services.ExamHistories
{
    public interface IExamHistoryService
    {
        Task<List<ExamHistoryDto>> GetHistoryByExam(long id);
        Task<List<MyHistoryDto>> GetMyHistory();
        Task<FinishExamDto> CompleteExam(CompleteExamDto completeExamDto);
    }

    public class ExamHistoryService : IExamHistoryService
    {
        //private readonly IHistoryExamRepository _historyExamRepository;
        //private readonly IAppUserRepository _appUserRepository;
        //private readonly IExamRepository _examRepository;
        //private readonly IQuestionRepository _questionRepository;
        //private readonly IAnswerQuestionRepository _answerQuestionRepository;
        private readonly IUnitOfWork _UOW;

        public ExamHistoryService(IUnitOfWork unitOfWork)
        {
            //_historyExamRepository = historyExamRepository;
            //_appUserRepository = appUserRepository;
            //_examRepository = examRepository;
            //_questionRepository = questionRepository;
            //_answerQuestionRepository = answerQuestionRepository;
            _UOW = unitOfWork;
        }

        
        public async Task<List<ExamHistoryDto>> GetHistoryByExam(long id)
        {
            var result = await _UOW.HistoryExamRepository.GetHistoryByExam(id);
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.AppUser = await _UOW.AppUserRepository.GetById(item.AppUserId);
                    item.AppUser.RefreshToken = null;
                }
            }
            return result;
        }

        public async Task<List<MyHistoryDto>> GetMyHistory()
        {
            var result = await _UOW.HistoryExamRepository.GetMyHistory();
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    var exam = await _UOW.ExamRepository.GetExamById(item.ExamId);
                    item.NameExam = exam.Name;
                }
            }
            return result;
        }

        public async Task<FinishExamDto> CompleteExam(CompleteExamDto completeExamDto)
        {
            var result = await _UOW.ExamRepository.GetExamById(completeExamDto.Id);
            if (result == null) throw new CustomException(ExamErrorEnum.ExamDoesNotExist);
            result.Questions = await _UOW.QuestionRepository.GetQuestionByExam(result.Id);
            int count = 0;
            double TotalRight = 0;
            foreach (var item in result.Questions)
            {
                item.Answers = await _UOW.AnswerQuestionRepository.getListByQuestion(item.Id);
                count++;    
            }

            foreach(var item in completeExamDto.Questions)
            {
                TotalRight += await CheckAnswer(completeExamDto.Id, item.Id, item.IdAnswers);
            }
            FinishExamDto exam = new FinishExamDto
            {
                Id = result.Id,
                Name = result.Name,
                TotalQuestion = count,
                CreateAt = DateTime.Now,
                CompleteTime = completeExamDto.CompleteTime,
                QuestionRight = TotalRight,
                ToTalScore = 10.0 * TotalRight/count
            };
            CreateHistoryExamDto examhistory = new CreateHistoryExamDto
            {
                ExamId = exam.Id,
                CompleteTime = exam.CompleteTime,
                QuestionRight = exam.QuestionRight,
                ToTalScore = exam.ToTalScore,
            };
            var myhistory = await _UOW.HistoryExamRepository.Create(examhistory);
            exam.CreateAt = myhistory.CreateAt;
            return exam;
            
        }

        public async Task<double> CheckAnswer(long idExam, long idQuestion, List<long> idAnswers)
        {
            var check = await _UOW.ExamRepository.CheckExamQuestion(idExam, idQuestion);
            var question = await _UOW.QuestionRepository.GetQuestionById(idQuestion);
            if (question == null || check == false) throw new CustomException(ExamErrorEnum.QuestionDoesNotMapExam);

            if (question.QuestionTypeId == 1)
            {
                if (idAnswers.Count > 1) throw new CustomException(ExamErrorEnum.InvalidQuestion);
                if (idAnswers.Count == 0) return 0;
                if (!await _UOW.AnswerQuestionRepository.CheckAnswerQuestion(question.Id, idAnswers[0])) throw new CustomException(ExamErrorEnum.AnswerDoesNotMapQuestion);
                var checkAnswer = await _UOW.AnswerQuestionRepository.Check(idAnswers[0]);
                if (checkAnswer) return 1;
                return 0;
            } else
            {
                var correctAnswer = await _UOW.AnswerQuestionRepository.CountAnswerCorrect(idQuestion);
                double score = (double)1.0 / correctAnswer;
                double total = 0;
                foreach (var item in idAnswers)
                {
                    if (!await _UOW.AnswerQuestionRepository.CheckAnswerQuestion(idQuestion, item)) throw new CustomException(ExamErrorEnum.AnswerDoesNotMapQuestion);
                    var checkAnswer = await _UOW.AnswerQuestionRepository.Check(item);
                    if (checkAnswer)
                    {
                        total += score;
                    }
                    else
                    {
                        total -= score;
                    }
                }
                return total;
            }
        }
    }
}
