using Microsoft.AspNetCore.Mvc;
using Store.Services.Abstractions;
using Store.Shard.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation
{
    [ApiController]
    [Route("API/[Controller]")]
    public class AuthController(IServiceManager _serviceManager): ControllerBase
    {
        //login
        [HttpPost("login")]//POST: baseUrl/api/auth/login
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _serviceManager.AuthService.LoginAsync(request);
            return Ok(result);


        }
        [HttpPost("register")]//POST: baseUrl/api/auth/register

        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _serviceManager.AuthService.RegisterAsync(request);
            return Ok(result);


        }


    }
}
