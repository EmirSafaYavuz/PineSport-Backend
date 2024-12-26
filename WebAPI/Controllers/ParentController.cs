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
    public class ParentController : BaseApiController
    {
        private readonly IParentService _parentService;

        public ParentController(IParentService parentService)
        {
            _parentService = parentService;
        }
        
        [HttpPost("register")]
        public IActionResult Register(ParentUserRegisterDto parentUserRegisterDto)
        {
            var result = _parentService.RegisterParent(parentUserRegisterDto);
            if (result.Success)
                return Success(result.Message, "Parent registered successfully", result);

            return BadRequest(result.Message, result.Message, result);
        }
    }
}
