using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse("Invalid Input"));
            try
            {
                var question = await _questionService.Create(createQuestionDto);
                return StatusCode(StatusCodes.Status201Created, new ResponseQuestion<QuestionDto>("Success", question));
                //return new OkObjectResult(new ResponseQuestion<QuestionDto>("Create question success", question));
            }
            catch (QuestionException ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new BaseResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
            }
        }

        [HttpPatch]
        public async Task<ActionResult<QuestionDto>> Update([FromBody] UpdateQuestionDto updateQuestionDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse("Invalid Input"));
            try
            {
                if (updateQuestionDto.StatusId != 3) return StatusCode(StatusCodes.Status403Forbidden, new BaseResponse("Can not be changed"));
                var question = await _questionService.Update(updateQuestionDto);
                return StatusCode(StatusCodes.Status200OK, new ResponseQuestion<QuestionDto>("Success", question));
            }
            catch (QuestionException ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new BaseResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAll()
        {
            try
            {
                var questions = await _questionService.GetAllQuestions();
                return new OkObjectResult(questions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
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
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse("Invalid Input"));
            try
            {
                var result = await _questionService.Delete(id);
                if (result == true)
                {
                    return StatusCode(StatusCodes.Status200OK, new BaseResponse("Success"));
                }
                else return StatusCode(StatusCodes.Status404NotFound, new BaseResponse("Not found question"));
            }
            catch (QuestionException ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new BaseResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
            }
        }
    }
}
