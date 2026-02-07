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

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createDto)
        {
            if (await _userService.UserExistsAsync(createDto.Email))
            {
                return Conflict();
            }

            var user = await _userService.CreateAsync(createDto.Email, createDto.Password);

            return Ok(new { token = _tokenService.Generate(user) });
        }
    }
}



