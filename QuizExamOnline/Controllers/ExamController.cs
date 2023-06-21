using Elasticsearch.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using QuizExamOnline.Common;
using QuizExamOnline.Entities;
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
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, "Invalid Input", "Dữ liệu truyền vào không hợp lệ", "BadRequest"));
            try
            {
                var exam = await _examService.CreateExam(createExamDto);
                return StatusCode(StatusCodes.Status201Created, new ResponseExam<ExamDto>("Success", exam));
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
        public async Task<ActionResult<ExamDto>> Update([FromBody] UpdateExamDto updateExamDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, "Invalid Input", "Dữ liệu truyền vào không hợp lệ", "BadRequest"));
            try
            {
                if (updateExamDto.StatusId != 3) return StatusCode(StatusCodes.Status403Forbidden, new ResponseException(403, "Can not change", "Không có quyền thay đỗi dữ liệu này", "Forbidden"));
                var exam = await _examService.Update(updateExamDto);
                return StatusCode(StatusCodes.Status200OK, new ResponseExam<ExamDto>("Success", exam));
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

        [AllowAnonymous]
        [HttpGet("getall")]
        public async Task<ActionResult<Paging<ExamDto>>> GetAll([FromQuery] int page = 1)
        {
            try
            {
                var exams = await _examService.GetAllExam(page);
                return new OkObjectResult(exams);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [AllowAnonymous]
        [HttpGet("getall_nopaging")]
        public async Task<ActionResult<IEnumerable<ExamDto>>> GetAllNopaging()
        {
            try
            {
                var exams = await _examService.GetAllExamNoPaging();
                return new OkObjectResult(exams);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
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
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ExamDto>> GetById(long id)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, "Invalid Input", "Dữ liệu truyền vào không hợp lệ", "BadRequest"));
            try
            {
                var exams = await _examService.GetExamById(id);
                return new OkObjectResult(exams);
            }
            catch (CustomException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseException(404, ex.Message, ex.Detail, "NotFound"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [AllowAnonymous]
        [HttpGet("getbycode/{code}")]
        public async Task<ActionResult<ExamDto>> GetByCode(string code)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, "Invalid Input", "Dữ liệu truyền vào không hợp lệ", "BadRequest"));
            try
            {
                var exams = await _examService.GetExamByCode(code);
                return new OkObjectResult(exams);
            }
            catch (CustomException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseException(404, ex.Message, ex.Detail, "NotFound"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<ActionResult<Paging<ExamDto>>> Search([FromQuery] string name = "", [FromQuery] int page = 1)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, "Invalid Input", "Dữ liệu truyền vào không hợp lệ", "BadRequest"));
            try
            {
                var exams = await _examService.Search(name, page);
                return new OkObjectResult(exams);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [AllowAnonymous]
        [HttpGet("search_nopaging")]
        public async Task<ActionResult<Paging<ExamDto>>> SearchNoPaging([FromQuery] string name = "")
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, "Invalid Input", "Dữ liệu truyền vào không hợp lệ", "BadRequest"));
            try
            {
                var exams = await _examService.SearchNoPaging(name);
                return new OkObjectResult(exams);
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
                var result = await _examService.Delete(id);
                if (result == true)
                {
                    return StatusCode(StatusCodes.Status200OK, new BaseResponse("Success"));
                }
                else return StatusCode(StatusCodes.Status404NotFound, new ResponseException(404, "Not found exam", "Không thể tìm thấy exam", "NotFound"));
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

        [HttpPost("complete")]
        public async Task<ActionResult<FinishExamDto>> Create([FromBody] CompleteExamDto completeExamDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, "Invalid Input", "Dữ liệu truyền vào không hợp lệ", "BadRequest"));
            try
            {
                var result = await _examHistoryService.CompleteExam(completeExamDto);
                return StatusCode(StatusCodes.Status200OK, new ResponseExam<FinishExamDto>("Success", result));
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

    }
}
