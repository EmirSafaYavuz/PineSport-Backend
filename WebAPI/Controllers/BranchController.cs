using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Dtos;
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
        
        [HttpPost("create")]
        public IActionResult Create([FromBody] BranchCreateDto branchCreateDto)
        {
            var result = _branchService.CreateBranch(branchCreateDto);

            if (result.Success)
                return Success(result.Message, "Branch registered successfully", result);

            return BadRequest(result.Message, result.Message);
        }
    }
}
