using System.Collections.Generic;
using Business.Abstract;
using Entities.Dtos.Register;
using Entities.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : BaseApiController
    {
        private readonly IStudentService _studentService;
        private readonly ISessionService _sessionService;

        public StudentsController(IStudentService studentService, ISessionService sessionService)
        {
            _studentService = studentService;
            _sessionService = sessionService;
        }
        
        [HttpGet]
        public IActionResult GetStudents()
        {
            var result = _studentService.GetStudents();
            return GetResponse(result);
        }
        
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var result = _studentService.GetStudentById(id);
            return GetResponse(result);
        }
        
        [HttpPost]
        public IActionResult RegisterStudent(StudentRegisterDto studentRegisterDto)
        {
            var result = _studentService.RegisterStudent(studentRegisterDto);
            return result.Success 
                ? Created(result.Message, "Student registered successfully") 
                : BadRequest(result.Message, "Failed to register student");
        }
        
        [HttpPut]
        public IActionResult UpdateStudent(StudentUpdateDto studentUpdateDto)
        {
            var result = _studentService.UpdateStudent(studentUpdateDto);
            return GetResponse(result);
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var result = _studentService.DeleteStudent(id);
            return GetResponseOnlyResult(result);
        }
        
        [HttpGet("{id}/sessions")]
        public IActionResult GetSessionsByStudentId(int id)
        {
            var result = _sessionService.GetSessionsByStudentId(id);
            return GetResponse(result); 
        }
        
        [HttpGet("search")]
        public IActionResult SearchStudents([FromQuery] string name)
        {
            var result = _studentService.SearchStudentsByName(name);
            return GetResponse(result);
        }
    }
}