using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Dtos;
using Auth.Interfaces;
using Auth.Models;
using Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto) 
        {
            var tokens = await _authService.LoginAsync(loginDto.Email, loginDto.Password);
            return Ok(tokens);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createDto)
        {
            if (await _userService.UserExistsAsync(createDto.Email))
            {
                return Conflict();
            }

            var user = await _userService.CreateAsync(createDto.Email, createDto.Password);
            var tokens = await _authService.LoginAsync(createDto.Email, createDto.Password);

            return Ok(tokens);
        }
    }
}



