using Business.Abstract;
using Entities.Dtos;
using Entities.Dtos.Register;
using Entities.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : BaseApiController
    {
        private readonly IBranchService _branchService;
        private readonly ISessionService _sessionService;
        private readonly IStudentService _studentService;
        private readonly ITrainerService _trainerService;

        public BranchesController(IBranchService branchService, ISessionService sessionService, IStudentService studentService, ITrainerService trainerService)
        {
            _branchService = branchService;
            _sessionService = sessionService;
            _studentService = studentService;
            _trainerService = trainerService;
        }

        // GET /api/branches
        [HttpGet]
        public IActionResult GetBranches()
        {
            var result = _branchService.GetBranches();
            return GetResponseOnlyResultData(result);
        }

        // GET /api/branches/{id}
        [HttpGet("{id}")]
        public IActionResult GetBranchById(int id)
        {
            var result = _branchService.GetBranchById(id);
            return GetResponseOnlyResultData(result);
        }

        // POST /api/branches
        [HttpPost]
        public IActionResult Register(BranchRegisterDto branchRegisterDto)
        {
            var result = _branchService.RegisterBranch(branchRegisterDto);
            if (result.Success)
            {
                return Created(result.Message, "Branch registered successfully", result);
            }

            return GetResponseOnlyResult(result);
        }

        // PUT /api/branches/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBranch(int id, [FromBody] BranchUpdateDto branchUpdateDto)
        {
            branchUpdateDto.Id = id;
            var result = _branchService.UpdateBranch(branchUpdateDto);
            return GetResponseOnlyResultData(result);
        }

        // DELETE /api/branches/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBranch(int id)
        {
            var result = _branchService.DeleteBranch(id);
            return GetResponseOnlyResultMessage(result);
        }

        // GET /api/branches/{id}/sessions
        [HttpGet("{id}/sessions")]
        public IActionResult GetBranchSessions(int id)
        {
            var result = _sessionService.GetSessionsByBranchId(id);
            return GetResponseOnlyResultData(result);
        }

        // GET /api/branches/{id}/students
        [HttpGet("{id}/students")]
        public IActionResult GetBranchStudents(int id)
        {
            var result = _studentService.GetStudentsByBranchId(id);
            return GetResponseOnlyResultData(result);
        }

        // GET /api/branches/{id}/trainers
        [HttpGet("{id}/trainers")]
        public IActionResult GetBranchTrainers(int id)
        {
            var result = _trainerService.GetTrainersByBranchId(id);
            return GetResponseOnlyResultData(result);
        }
    }
}