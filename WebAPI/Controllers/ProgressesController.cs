using Business.Abstract;
using Entities.Dtos.Register;
using Entities.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressesController : BaseApiController
    {
        private readonly IProgressService _progressService;

        public ProgressesController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        // GET /api/progresses
        [HttpGet]
        public IActionResult GetAllProgresses()
        {
            var result = _progressService.GetAllProgresses();
            return GetResponse(result);
        }

        // POST /api/progresses
        [HttpPost]
        public IActionResult CreateProgress([FromBody] ProgressCreateDto progressCreateDto)
        {
            var result = _progressService.CreateProgress(progressCreateDto);
            return result.Success
                ? Created(result.Message, "Progress created successfully.")
                : BadRequest(result.Message, result.Message);
        }

        // GET /api/progresses/{studentId}
        [HttpGet("{studentId}")]
        public IActionResult GetProgressByStudentId(int studentId)
        {
            var result = _progressService.GetProgressByStudentId(studentId);
            return GetResponse(result);
        }

        // PUT /api/progresses/{studentId}
        [HttpPut("{studentId}")]
        public IActionResult UpdateProgress(int studentId, [FromBody] ProgressUpdateDto progressUpdateDto)
        {
            progressUpdateDto.StudentId = studentId; // Ensure the StudentId is set in the DTO
            var result = _progressService.UpdateProgress(progressUpdateDto);
            return GetResponseOnlyResult(result);
        }

        // DELETE /api/progresses/{studentId}
        [HttpDelete("{studentId}")]
        public IActionResult DeleteProgress(int studentId)
        {
            var result = _progressService.DeleteProgress(studentId);
            return GetResponseOnlyResult(result);
        }
    }
}