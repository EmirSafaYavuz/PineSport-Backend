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
    public class SchoolController : BaseApiController
    {
        private readonly ISchoolService _schoolService;
        private readonly IBranchService _branchService;

        public SchoolController(ISchoolService schoolService, IBranchService branchService)
        {
            _schoolService = schoolService;
            _branchService = branchService;
        }
        
        [HttpPost]
        public IActionResult Register([FromBody] SchoolRegisterDto schoolRegisterDto)
        {
            var result = _schoolService.RegisterSchool(schoolRegisterDto);

            if (result.Success)
                return Success(result.Message, "School registered successfully", result);

            return BadRequest(result.Message, result.Message, result);
        }
        
        [HttpGet]
        public IActionResult GetSchools()
        {
            var result = _schoolService.GetSchools();
            if (result.Success)
            {
                return Success(result.Message, "Schools listed successfully", result.Data);
            }

            return BadRequest(result.Message, result.Message, result.Data);
        }
        
        [HttpGet("{schoolId}")]
        public IActionResult GetSchoolById(int schoolId)
        {
            var result = _schoolService.GetSchoolById(schoolId);
            if (result.Success)
            {
                return Success(result.Message, "School listed successfully", result.Data);
            }

            return BadRequest(result.Message, result.Message, result.Data);
        }
        
        [HttpGet("{schoolId}/branches")]
        public IActionResult GetSchoolBranches(int schoolId)
        {
            var result = _branchService.GetBranchesBySchoolId(schoolId);
            if (result.Success)
            {
                return Success(result.Message, "Branches listed successfully", result.Data);
            }

            return BadRequest(result.Message, result.Message, result.Data);
        }
    }
}
