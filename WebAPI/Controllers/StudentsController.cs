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
    public class StudentController : BaseApiController
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        
        [HttpGet]
        public IActionResult GetStudents()
        {
            var result = _studentService.GetStudents();
            if (result.Success)
            {
                return Success(result.Message, "Students listed successfully", result.Data);
            }

            return BadRequest(result.Message, result.Message, result.Data);
        }
        
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var result = _studentService.GetStudentById(id);
            if (result.Success)
            {
                return Success(result.Message, "Student listed successfully", result.Data);
            }

            return BadRequest(result.Message, result.Message, result.Data);
        }
        
        [HttpPost]
        public IActionResult RegisterStudent(StudentRegisterDto studentRegisterDto)
        {
            var result = _studentService.RegisterStudent(studentRegisterDto);
            if (result.Success)
            {
                return Created(result.Message, "Student registered successfully");
            }

            return BadRequest(result.Message, result.Message);
        }
    }
}
