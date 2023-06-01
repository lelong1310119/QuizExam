using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using QuizExamOnline.Entities.Histories;
using QuizExamOnline.Entities.Questions;
using QuizExamOnline.Responses;
using QuizExamOnline.Services.ExamHistories;

namespace QuizExamOnline.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class ExamHistoryController : ControllerBase
    {
        private readonly IExamHistoryService _examHistoryService;

        public ExamHistoryController(IExamHistoryService examHistoryService)
        {
            _examHistoryService = examHistoryService;
        }

        //[AllowAnonymous]
        [HttpGet("historyexam/{id}")]
        public async Task<ActionResult<IEnumerable<ExamHistoryDto>>> GetHistoryExam(long id)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse("Invalid Input"));
            try
            {
                var histories = await _examHistoryService.GetHistoryByExam(id);
                return new OkObjectResult(histories);
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
            }
        }

        //[AllowAnonymous]
        [HttpGet("historyuser")]
        public async Task<ActionResult<IEnumerable<ExamHistoryDto>>> GetMyHistory()
        {
            try
            {
                var histories = await _examHistoryService.GetMyHistory();
                return new OkObjectResult(histories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
            }
        }
    }
}
