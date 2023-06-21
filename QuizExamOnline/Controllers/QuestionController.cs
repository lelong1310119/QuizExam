using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizExamOnline.Common;
using QuizExamOnline.Entities;
using QuizExamOnline.Entities.AnswerQuestions;
using QuizExamOnline.Entities.AppUsers;
using QuizExamOnline.Entities.Exams;
using QuizExamOnline.Entities.Questions;
using QuizExamOnline.Models;
using QuizExamOnline.Repositories;
using QuizExamOnline.Responses;
using QuizExamOnline.Services.AppUsers;
using QuizExamOnline.Services.Questions;

namespace QuizExamOnline.Controllers
{
    [Route("api/questions")]
    [ApiController]
    [Authorize]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
           _questionService = questionService;
        }

        [HttpPost]
        public async Task<ActionResult<QuestionDto>> Create([FromBody] CreateQuestionDto createQuestionDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, "Invalid Input", "Dữ liệu truyền vào không hợp lệ", "BadRequest"));
            try
            {
                var question = await _questionService.Create(createQuestionDto);
                return StatusCode(StatusCodes.Status201Created, new ResponseQuestion<QuestionDto>("Success", question));
                //return new OkObjectResult(new ResponseQuestion<QuestionDto>("Create question success", question));
            }
            catch (CustomException ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new ResponseException(422, ex.Message, ex.Detail, "UnprocessableEntity"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [HttpPatch]
        public async Task<ActionResult<QuestionDto>> Update([FromBody] UpdateQuestionDto updateQuestionDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, "Invalid Input", "Dữ liệu truyền vào không hợp lệ", "BadRequest"));
            try
            {
                if (updateQuestionDto.StatusId != 3) return StatusCode(StatusCodes.Status403Forbidden, new ResponseException(403, "Can not change", "Không có quyền thay đỗi dữ liệu này", "Forbidden"));
                var question = await _questionService.Update(updateQuestionDto);
                return StatusCode(StatusCodes.Status200OK, new ResponseQuestion<QuestionDto>("Success", question));
            }
            catch (CustomException ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new ResponseException(422, ex.Message, ex.Detail, "UnprocessableEntity"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [HttpGet]
        public async Task<ActionResult<Paging<QuestionDto>>> GetAll([FromQuery] int page = 1)
        {
            try
            {
                var questions = await _questionService.GetAllQuestions(page);
                return new OkObjectResult(questions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [HttpGet("getall_nopaging")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAllNoPaging()
        {
            try
            {
                var questions = await _questionService.GetAllQuestionsNoPaging();
                return new OkObjectResult(questions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<Paging<QuestionDto>>> Search([FromQuery] string filter = "", [FromQuery] int page = 1)
        {
            try
            {
                var questions = await _questionService.Search(filter, page);
                return new OkObjectResult(questions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [HttpGet("search_nopaging")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> SearchNoPaging([FromQuery] string filter = "")
        {
            try
            {
                var questions = await _questionService.SearchNoPaging(filter);
                return new OkObjectResult(questions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [HttpGet("questionusers")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAllByUser()
        {
            try
            {
                var questions = await _questionService.GetListQuestionsUser();
                return new OkObjectResult(questions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, "Invalid Input", "Dữ liệu truyền vào không hợp lệ", "BadRequest"));
            try
            {
                var result = await _questionService.Delete(id);
                if (result == true)
                {
                    return StatusCode(StatusCodes.Status200OK, new BaseResponse("Success"));
                }
                else return StatusCode(StatusCodes.Status404NotFound, new ResponseException(404, "Not found question", "Không thể tìm thấy question", "NotFound"));
            }
            catch (CustomException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ResponseException(403, ex.Message, ex.Detail, "Forbidden"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }
    }
}
