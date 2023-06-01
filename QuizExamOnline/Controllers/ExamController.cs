using Elasticsearch.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using QuizExamOnline.Entities.Exams;
using QuizExamOnline.Entities.Histories;
using QuizExamOnline.Entities.Questions;
using QuizExamOnline.Repositories;
using QuizExamOnline.Responses;
using QuizExamOnline.Services.ExamHistories;
using QuizExamOnline.Services.Exams;
using QuizExamOnline.Services.Questions;
using TrueSight.Common;

namespace QuizExamOnline.Controllers
{
    [Route("api/exams")]
    [ApiController]
    [Authorize]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;
        private readonly IExamHistoryService    _examHistoryService;
        public ExamController(IExamService examService, IExamHistoryService examHistoryService)
        {
            _examService = examService;
            _examHistoryService = examHistoryService;
        }

        [HttpPost]
        public async Task<ActionResult<ExamDto>> Create([FromBody] CreateExamDto createExamDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse("Invalid Input"));
            try
            {
                var exam = await _examService.CreateExam(createExamDto);
                return StatusCode(StatusCodes.Status201Created, new ResponseExam<ExamDto>("Success", exam));
            }
            catch (ExamException ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new BaseResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
            }
        }

        [HttpPatch]
        public async Task<ActionResult<ExamDto>> Update([FromBody] UpdateExamDto updateExamDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse("Invalid Input"));
            try
            {
                if (updateExamDto.StatusId != 3) return StatusCode(StatusCodes.Status403Forbidden, new BaseResponse("Can not be changed"));
                var exam = await _examService.Update(updateExamDto);
                return StatusCode(StatusCodes.Status200OK, new ResponseExam<ExamDto>("Success", exam));
            }
            catch (ExamException ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new BaseResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamDto>>> GetAll()
        {
            try
            {
                var exams = await _examService.GetAllExam();
                return new OkObjectResult(exams);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
            }
        }

        [HttpGet("examusers")]
        public async Task<ActionResult<IEnumerable<ExamDto>>> GetAllByUser()
        {
            try
            {
                var exams = await _examService.GetExamByUser();
                return new OkObjectResult(exams);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExamDto>> GetById(long id)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse("Invalid Input"));
            try
            {
                var exams = await _examService.GetExamById(id);
                return new OkObjectResult(exams);
            }
            catch (ExamException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new BaseResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
            }
        }

        [HttpGet("getbycode/{code}")]
        public async Task<ActionResult<ExamDto>> GetByCode(string code)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse("Invalid Input"));
            try
            {
                var exams = await _examService.GetExamByCode(code);
                return new OkObjectResult(exams);
            }
            catch (ExamException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new BaseResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpGet("search/{name}")]
        public async Task<ActionResult<IEnumerable<ExamDto>>> Search(string name)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse("Invalid Input"));
            try
            {
                var exams = await _examService.Search(name);
                return new OkObjectResult(exams);
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
                var result = await _examService.Delete(id);
                if (result == true)
                {
                    return StatusCode(StatusCodes.Status200OK, new BaseResponse("Success"));
                }
                else return StatusCode(StatusCodes.Status404NotFound, new BaseResponse("Not found exam"));
            }
            catch (ExamException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new BaseResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
            }
        }

        [HttpPost("complete")]
        public async Task<ActionResult<FinishExamDto>> Create([FromBody] CompleteExamDto completeExamDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse("Invalid Input"));
            try
            {
                var result = await _examHistoryService.CompleteExam(completeExamDto);
                return StatusCode(StatusCodes.Status200OK, new ResponseExam<FinishExamDto>("Success", result));
            }
            catch (ExamException ex)
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
