using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todo_webapi.Core.Entities;
using TodoApp.Application.DTO;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Services;

namespace todo_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IApplicationUserService _userService;

        public UserController(IApplicationUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            var user = new ApplicationUser
            {
                UserName = createUserDto.UserName,
                Email = createUserDto.Email
            };

            await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, MapToDto(user));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(MapToDto(user));
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userService.GetAllUsersAsync();
            if (users == null) return NotFound();
            return Ok(users.Select(MapToDto));
        }

        private static UserDto MapToDto(ApplicationUser user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }
}

