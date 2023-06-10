using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizExamOnline.Entities;
using QuizExamOnline.Responses;
using QuizExamOnline.Services;

namespace QuizExamOnline.Controllers
{
    [Route("api")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly IGeneralService _generalService;
        public GeneralController(IGeneralService generalService) {
            _generalService = generalService;
        }

        [HttpGet("grades")]
        public async Task<ActionResult<IEnumerable<EntityEnumDto>>> GetListGrade()
        {
            try
            {
                var result = await _generalService.getListGrade();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [HttpGet("statuses")]
        public async Task<ActionResult<IEnumerable<EntityEnumDto>>> GetListStatus()
        {
            try
            {
                var result = await _generalService.getListStatus();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [HttpGet("levels")]
        public async Task<ActionResult<IEnumerable<EntityEnumDto>>> GetListLevel()
        {
            try
            {
                var result = await _generalService.getListLevel();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [HttpGet("questiontypes")]
        public async Task<ActionResult<IEnumerable<EntityEnumDto>>> GetListQuestionType()
        {
            try
            {
                var result = await _generalService.getListQuestionType();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [HttpGet("questiongroups")]
        public async Task<ActionResult<IEnumerable<EntityEnumDto>>> GetListQuestionGroup()
        {
            try
            {
                var result = await _generalService.getListQuestionGroup();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }

        [HttpGet("subjects")]
        public async Task<ActionResult<IEnumerable<EntityEnumDto>>> GetListSubject()
        {
            try
            {
                var result = await _generalService.getListSubject();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseException(400, ex.Message, "", "BadRequest"));
            }
        }
    }
}
