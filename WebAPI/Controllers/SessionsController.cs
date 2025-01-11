using Business.Abstract;
using Entities.Dtos;
using Entities.Dtos.Register;
using Entities.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : BaseApiController
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        // GET /api/sessions
        [HttpGet]
        public IActionResult GetSessions()
        {
            var result = _sessionService.GetSessions();
            return GetResponseOnlyResultData(result);
        }

        // POST /api/sessions
        [HttpPost]
        public IActionResult CreateSession([FromBody] SessionCreateDto sessionCreateDto)
        {
            var result = _sessionService.CreateSession(sessionCreateDto);
            return result.Success 
                ? Created(result.Message, "Session created successfully") 
                : BadRequest(result.Message, result.Message);
        }

        // GET /api/sessions/{id}
        [HttpGet("{id}")]
        public IActionResult GetSessionById(int id)
        {
            var result = _sessionService.GetSessionById(id);
            return GetResponseOnlyResultData(result);
        }

        // PUT /api/sessions/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateSession(int id, [FromBody] SessionUpdateDto sessionUpdateDto)
        {
            sessionUpdateDto.Id = id; // Ensure session ID is passed to the service
            var result = _sessionService.UpdateSession(sessionUpdateDto);
            return GetResponseOnlyResultMessage(result);
        }

        // DELETE /api/sessions/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteSession(int id)
        {
            var result = _sessionService.DeleteSession(id);
            return GetResponseOnlyResult(result);
        }

        // POST /api/sessions/{id}/assign-student
        [HttpPost("{id}/assign-student")]
        public IActionResult AssignStudentToSession(int id, [FromBody] AssignStudentDto assignStudentDto)
        {
            assignStudentDto.SessionId = id; // Ensure session ID is passed
            var result = _sessionService.AssignStudentToSession(assignStudentDto);
            return GetResponseOnlyResult(result);
        }

        // POST /api/sessions/{id}/assign-coach
        [HttpPost("{id}/assign-trainer")]
        public IActionResult AssignTrainerToSession(int id, [FromBody] AssignCoachDto assignCoachDto)
        {
            assignCoachDto.SessionId = id; // Ensure session ID is passed
            var result = _sessionService.AssignTrainerToSession(assignCoachDto);
            return GetResponseOnlyResult(result);
        }
    }
}