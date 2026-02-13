using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Dtos;
using Auth.Interfaces;
using Auth.Models;
using Auth.Services;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout() 
        {
            var id = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
            {
                return Unauthorized();
            }

            if (!Guid.TryParse(id, out var guid))
            {
                return Unauthorized();
            }

            await _authService.LogoutAsync(guid);

            return Ok();
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

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken) 
        {
            var tokens = await _authService.RefreshAccessToken(refreshToken);
            return Ok(tokens);
        }
    }
}



