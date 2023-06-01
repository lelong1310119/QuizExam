using Nest;
using QuizExamOnline.Entities.Histories;
using QuizExamOnline.Repositories;
using QuizExamOnline.Services.Exams;

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
        private readonly IHistoryExamRepository _historyExamRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerQuestionRepository _answerQuestionRepository;

        public ExamHistoryService(IHistoryExamRepository historyExamRepository, IAppUserRepository appUserRepository, IExamRepository examRepository, IQuestionRepository questionRepository, IAnswerQuestionRepository answerQuestionRepository)
        {
            _historyExamRepository = historyExamRepository;
            _appUserRepository = appUserRepository;
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _answerQuestionRepository = answerQuestionRepository;
        }

        
        public async Task<List<ExamHistoryDto>> GetHistoryByExam(long id)
        {
            var result = await _historyExamRepository.GetHistoryByExam(id);
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.AppUser = await _appUserRepository.GetById(item.AppUserId);
                }
            }
            return result;
        }

        public async Task<List<MyHistoryDto>> GetMyHistory()
        {
            var result = await _historyExamRepository.GetMyHistory();
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    var exam = await _examRepository.GetExamById(item.ExamId);
                    item.NameExam = exam.Name;
                }
            }
            return result;
        }

        public async Task<FinishExamDto> CompleteExam(CompleteExamDto completeExamDto)
        {
            var result = await _examRepository.GetExamById(completeExamDto.Id);
            if (result == null) throw new ExamException("Not fount exam");
            result.Questions = await _questionRepository.GetQuestionByExam(result.Id);
            int count = 0;
            double TotalRight = 0;
            foreach (var item in result.Questions)
            {
                item.Answers = await _answerQuestionRepository.getListByQuestion(item.Id);
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
            var myhistory = await _historyExamRepository.Create(examhistory);
            exam.CreateAt = myhistory.CreateAt;
            return exam;
            
        }

        public async Task<double> CheckAnswer(long idExam, long idQuestion, List<long> idAnswers)
        {
            var check = await _examRepository.CheckExamQuestion(idExam, idQuestion);
            var question = await _questionRepository.GetQuestionById(idQuestion);
            if (question == null || check == false) throw new ExamException("Invalid Question");

            if (question.QuestionTypeId == 1)
            {
                if (idAnswers.Count > 1) throw new ExamException("Invalid Question");
                if (idAnswers.Count == 0) return 0;
                var checkAnswer = await _answerQuestionRepository.Check(idAnswers[0]);
                if (checkAnswer) return 1;
                return 0;
            } else
            {
                if (idAnswers.Count < 2) throw new ExamException("Invalid Question");
                var correctAnswer = await _answerQuestionRepository.CountAnswerCorrect(idQuestion);
                double score = (double)1.0 / correctAnswer;
                double total = 0;
                foreach (var item in idAnswers)
                {
                    if (!await _answerQuestionRepository.CheckAnswerQuestion(idQuestion, item)) throw new ExamException("Invalid Question");
                    var checkAnswer = await _answerQuestionRepository.Check(item);
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
