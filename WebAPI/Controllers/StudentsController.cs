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

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
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
    }
}