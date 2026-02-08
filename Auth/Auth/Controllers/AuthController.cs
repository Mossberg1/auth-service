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
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;

        public AuthController(IUserService userService, ITokenService tokenService, IAuthService authService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto) 
        {
            var accessToken = await _authService.LoginAsync(loginDto.Email, loginDto.Password);
            return Ok(new { token = accessToken });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createDto)
        {
            if (await _userService.UserExistsAsync(createDto.Email))
            {
                return Conflict();
            }

            var user = await _userService.CreateAsync(createDto.Email, createDto.Password);
            var accessToken = _tokenService.Generate(user);

            return Ok(new { token =  accessToken });
        }
    }
}



