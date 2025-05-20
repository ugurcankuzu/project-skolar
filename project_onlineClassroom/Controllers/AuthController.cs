using Microsoft.AspNetCore.Mvc;
using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;
using project_onlineClassroom.Repositories;
using project_onlineClassroom.Util;

namespace project_onlineClassroom.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(LoginDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(LoginDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(LoginDTO), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new LoginDTO(false, "Invalid login request"));
                }
                string token = await _authService.LoginAsync(request);
                return Ok(new LoginDTO(token));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new LoginDTO(false, ex.Message));
            }
            catch (InvalidPasswordException ex)
            {
                return Unauthorized(new LoginDTO(false, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new LoginDTO(false, ex.Message));
            }

        }
        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RegisterDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RegisterDTO), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(RegisterDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RegisterDTO(false, "Invalid registration request"));
                }
                await _authService.RegisterAsync(request);
                return Ok(new RegisterDTO(success: true, "Registration completed successfully!"));
            }
            catch (PasswordMismatchException ex)
            {
                return BadRequest(new RegisterDTO(false, ex.Message));
            }
            catch (UserExistsException ex)
            {
                return Conflict(new RegisterDTO(false, ex.Message));
            }
            catch (InvalidEmailException ex)
            {
                return BadRequest(new RegisterDTO(false, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RegisterDTO(false, ex.Message));
            }

        }
    }
}
