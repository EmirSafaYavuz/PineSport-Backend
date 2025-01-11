using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Dtos;
using Entities.Dtos.Register;
using Entities.Dtos.Update;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController : BaseApiController
    {
        private readonly ISchoolService _schoolService;
        private readonly IBranchService _branchService;

        public SchoolsController(ISchoolService schoolService, IBranchService branchService)
        {
            _schoolService = schoolService;
            _branchService = branchService;
        }
        
        [HttpPost]
        public IActionResult Register([FromBody] SchoolRegisterDto schoolRegisterDto)
        {
            var result = _schoolService.RegisterSchool(schoolRegisterDto);

            if (result.Success)
            {
                return Created(result.Message, "School registered successfully", result);
            }

            return BadRequest(result.Message, result.Message);
        }
        
        [HttpGet]
        public IActionResult GetSchools()
        {
            var result = _schoolService.GetSchools();
            return GetResponseOnlyResultData(result);
        }
        
        [HttpGet("{schoolId}")]
        public IActionResult GetSchoolById(int schoolId)
        {
            var result = _schoolService.GetSchoolById(schoolId);
            return GetResponseOnlyResultData(result);
        }
        
        [HttpGet("{schoolId}/branches")]
        public IActionResult GetSchoolBranches(int schoolId)
        {
            var result = _branchService.GetBranchesBySchoolId(schoolId);
            return GetResponseOnlyResultData(result);
        }
        
        [HttpPut]
        public IActionResult UpdateSchool(SchoolUpdateDto schoolUpdateDto)
        {
            var result = _schoolService.UpdateSchool(schoolUpdateDto);
            return GetResponse(result);
        }
        
        [HttpDelete("{schoolId}")]
        public IActionResult DeleteSchool(int schoolId)
        {
            var result = _schoolService.DeleteSchool(schoolId);
            return GetResponseOnlyResult(result);
        }
    }
}
