using Business.Abstract;
using Entities.Dtos;
using Entities.Dtos.Register;
using Entities.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentsController : BaseApiController
    {
        private readonly IParentService _parentService;
        private readonly IStudentService _studentService;

        public ParentsController(IParentService parentService, IStudentService studentService)
        {
            _parentService = parentService;
            _studentService = studentService;
        }

        // GET /api/parents
        [HttpGet]
        public IActionResult GetParents()
        {
            var result = _parentService.GetParents();
            return GetResponseOnlyResultData(result);
        }

        // GET /api/parents/{id}
        [HttpGet("{id}")]
        public IActionResult GetParentById(int id)
        {
            var result = _parentService.GetParentById(id);
            return GetResponseOnlyResultData(result);
        }

        // POST /api/parents
        [HttpPost]
        public IActionResult RegisterParent(ParentRegisterDto parentRegisterDto)
        {
            var result = _parentService.RegisterParent(parentRegisterDto);
            return result.Success 
                ? Created(result.Message, "Parent registered successfully") 
                : BadRequest(result.Message, result.Message);
        }

        // PUT /api/parents/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateParent(int id, ParentUpdateDto parentUpdateDto)
        {
            parentUpdateDto.Id = id; // Ensure ID is passed to the service layer
            var result = _parentService.UpdateParent(parentUpdateDto);
            return GetResponseOnlyResultData(result);
        }

        // DELETE /api/parents/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteParent(int id)
        {
            var result = _parentService.DeleteParent(id);
            return GetResponseOnlyResult(result);
        }

        // GET /api/parents/{id}/students
        [HttpGet("{id}/students")]
        public IActionResult GetStudentsByParentId(int id)
        {
            var result = _studentService.GetStudentsByParentId(id);
            return GetResponseOnlyResultData(result);
        }

        // GET /api/parents/search?name={name}
        [HttpGet("search")]
        public IActionResult SearchParents([FromQuery] string name)
        {
            var result = _parentService.SearchParentsByName(name);
            return GetResponseOnlyResultData(result);
        }
    }
}