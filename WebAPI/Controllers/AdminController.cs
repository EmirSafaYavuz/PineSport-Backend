using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Dtos;
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
        public IActionResult RegisterAdmin([FromBody] AdminUserRegisterDto adminUserRegisterDto)
        {
            var result = _adminService.RegisterAdmin(adminUserRegisterDto);

            if (result.Success)
                return Success(result.Message, "Admin registered successfully", result);

            return BadRequest(result.Message, result.Message, result);
        }
    }
}
