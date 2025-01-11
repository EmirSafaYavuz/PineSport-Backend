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
    public class ParentsController : BaseApiController
    {
        private readonly IParentService _parentService;

        public ParentsController(IParentService parentService)
        {
            _parentService = parentService;
        }
        
        [HttpGet]
        public IActionResult GetParents()
        {
            var result = _parentService.GetParents();
            if (result.Success)
                return Success(result.Message, "Parents listed successfully", result.Data);

            return BadRequest(result.Message, result.Message, result.Data);
        }
        
        [HttpGet("{parentId}")]
        public IActionResult GetParentById(int parentId)
        {
            var result = _parentService.GetParentById(parentId);
            if (result.Success)
                return Success(result.Message, "Parent retrieved successfully", result.Data);

            return BadRequest(result.Message, result.Message, result.Data);
        }
        
        [HttpPost]
        public IActionResult RegisterParent(ParentRegisterDto parentRegisterDto)
        {
            var result = _parentService.RegisterParent(parentRegisterDto);
            if (result.Success)
                return Created(result.Message, "Parent registered successfully", result);

            return BadRequest(result.Message, result.Message, result);
        }
    }
}
