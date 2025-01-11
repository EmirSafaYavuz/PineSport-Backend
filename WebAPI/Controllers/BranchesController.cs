using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Dtos;
using Entities.Dtos.Register;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : BaseApiController
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }
        
        [HttpGet]
        public IActionResult GetBranches()
        {
            var result = _branchService.GetBranches();
            if (result.Success)
            {
                return Success(result.Message, "Branches listed successfully", result.Data);
            }

            return BadRequest(result.Message, result.Message, result.Data);
        }
        
        [HttpGet("{branchId}")]
        public IActionResult GetBranchById(int branchId)
        {
            var result = _branchService.GetBranchById(branchId);
            if (result.Success)
            {
                return Success(result.Message, "Branch listed successfully", result.Data);
            }

            return BadRequest(result.Message, result.Message, result.Data);
        }
        
        [HttpPost]
        public IActionResult Register([FromBody] BranchRegisterDto branchRegisterDto)
        {
            var result = _branchService.RegisterBranch(branchRegisterDto);

            if (result.Success)
                return Success(result.Message, "Branch registered successfully", result);

            return BadRequest(result.Message, result.Message);
        }
    }
}
