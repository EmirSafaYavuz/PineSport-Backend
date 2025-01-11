using Business.Abstract;
using Entities.Dtos;
using Entities.Dtos.Register;
using Entities.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainersController : BaseApiController
    {
        private readonly ITrainerService _trainerService;
        private readonly ISessionService _sessionService;
        private readonly IBranchService  _branchService;

        public TrainersController(ITrainerService trainerService, ISessionService sessionService, IBranchService branchService)
        {
            _trainerService = trainerService;
            _sessionService = sessionService;
            _branchService = branchService;
        }

        // GET /api/trainers
        [HttpGet]
        public IActionResult GetTrainers()
        {
            var result = _trainerService.GetTrainers();
            return GetResponseOnlyResultData(result);
        }

        // GET /api/trainers/{id}
        [HttpGet("{id}")]
        public IActionResult GetTrainerById(int id)
        {
            var result = _trainerService.GetTrainerById(id);
            return GetResponseOnlyResultData(result);
        }

        // POST /api/trainers
        [HttpPost]
        public IActionResult RegisterTrainer([FromBody] TrainerRegisterDto trainerRegisterDto)
        {
            var result = _trainerService.RegisterTrainer(trainerRegisterDto);
            return result.Success 
                ? Created(result.Message, "Trainer registered successfully") 
                : BadRequest(result.Message, result.Message);
        }

        // PUT /api/trainers/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateTrainer(int id, [FromBody] TrainerUpdateDto trainerUpdateDto)
        {
            trainerUpdateDto.Id = id; // Ensure the trainer ID is passed to the service layer
            var result = _trainerService.UpdateTrainer(trainerUpdateDto);
            return GetResponseOnlyResultData(result);
        }

        // DELETE /api/trainers/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteTrainer(int id)
        {
            var result = _trainerService.DeleteTrainer(id);
            return GetResponseOnlyResult(result);
        }

        // GET /api/trainers/{id}/sessions
        [HttpGet("{id}/sessions")]
        public IActionResult GetTrainerSessions(int id)
        {
            var result = _sessionService.GetSessionsByTrainerId(id);
            return GetResponseOnlyResultData(result);
        }

        // GET /api/trainers/{id}/branches
        [HttpGet("{id}/branches")]
        public IActionResult GetTrainerBranches(int id)
        {
            var result = _branchService.GetBranchesByTrainerId(id);
            return GetResponseOnlyResultData(result);
        }

        // GET /api/trainers/search?name={name}
        [HttpGet("search")]
        public IActionResult SearchTrainers([FromQuery] string name)
        {
            var result = _trainerService.SearchTrainersByName(name);
            return GetResponseOnlyResultData(result);
        }
    }
}