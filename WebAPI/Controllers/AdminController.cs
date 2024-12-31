using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Dtos;
using Entities.Dtos.Register;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : BaseApiController
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        
        [HttpPost("register")]
        public IActionResult RegisterAdmin([FromBody] AdminRegisterDto adminRegisterDto)
        {
            var result = _adminService.RegisterAdmin(adminRegisterDto);

            if (result.Success)
                return Success(result.Message, "Admin registered successfully", result);

            return BadRequest(result.Message, result.Message, result);
        }
    }
}
