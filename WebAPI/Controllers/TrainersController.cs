using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Dtos.Register;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerController : BaseApiController
    {
        
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        [HttpGet]
        public IActionResult GetTrainers()
        {
            var result = _trainerService.GetTrainers();
            if (result.Success)
            {
                return Success(result.Message, "Trainers listed successfully", result.Data);
            }

            return BadRequest(result.Message, result.Message, result.Data);
        }
        
        [HttpGet("{id}")]
        public IActionResult GetTrainerById(int id)
        {
            var result = _trainerService.GetTrainerById(id);
            if (result.Success)
            {
                return Success(result.Message, "Trainer listed successfully", result.Data);
            }

            return BadRequest(result.Message, result.Message, result.Data);
        }
        
        [HttpPost]
        public IActionResult RegisterTrainer(TrainerRegisterDto trainerRegisterDto)
        {
            var result = _trainerService.RegisterTrainer(trainerRegisterDto);
            if (result.Success)
            {
                return Created(result.Message, "Trainer registered successfully");
            }

            return BadRequest(result.Message, result.Message);
        }
    }
}
