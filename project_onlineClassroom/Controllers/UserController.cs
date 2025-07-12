
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs;
using project_onlineClassroom.DTOs.UserDTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;
using project_onlineClassroom.Repositories;

namespace project_onlineClassroom.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) : base() => _userService = userService;

        // GET: User
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                string? userId = User.FindFirstValue("id");
                if (String.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id)) throw new InvalidTokenException();
                User user = await _userService.GetUserProfileByIdAsync(id);
                return Ok(new GenericResponse<ProfileDTO>(new ProfileDTO(user)));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new GenericResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse<object>(ex.Message));
            }
        }
        [HttpPatch("me")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDTO updateRequest)
        {
            try
            {
                string? userId = User.FindFirstValue("id");
                if (String.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id)) throw new InvalidTokenException();
                User user = await _userService.UpdateUserProfileAsync(id, updateRequest);
                return Ok(new GenericResponse<ProfileDTO>(new ProfileDTO(user)));

            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new GenericResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse<object>(ex.Message));
            }
        }
        [HttpDelete("me")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                string? userId = User.FindFirstValue("id");
                if (String.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id)) throw new InvalidTokenException();
                await _userService.DeleteUser(id);
                return Ok(new GenericResponse<object>(true, "User deleted successfully."));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new GenericResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse<object>(ex.Message));
            }
        }
        [HttpGet("me/verify-token")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status401Unauthorized)]
        public IActionResult VerifyToken()
        {
            try
            {
                string? userId = User.FindFirstValue("id");
                if (String.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id)) throw new InvalidTokenException();
                return Ok(new GenericResponse<object>(true, "Token is valid."));
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new GenericResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse<object>(ex.Message));
            }
        }
        [HttpGet("me/resolve-token")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ResolveToken()
        {
            try
            {
                string? userId = User.FindFirstValue("id");
                if (String.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id)) throw new InvalidTokenException();
                User user = await _userService.GetUserProfileByIdAsync(id);
                return Ok(new GenericResponse<ProfileDTO>(new ProfileDTO(user)));
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new GenericResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse<object>(ex.Message));
            }
        }
        [HttpPatch("me/role-select")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SelectRole([FromBody] RoleSelectRequest req)
        {
            try
            {
                Console.WriteLine("ROLE SEÇİM İSTEĞİ: " + req.IsEducator);
                string? userId = User.FindFirstValue("id");
                if (String.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id)) throw new InvalidTokenException();
                User updatedUser = await _userService.UpdateUserFirstLoginAsync(id, req.IsEducator);
                ProfileDTO profile = new ProfileDTO(updatedUser);
                return Ok(new GenericResponse<ProfileDTO>(profile));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new GenericResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse<object>(ex.Message));
            }
        }
    }
}

/*
    TODO: Custom Errorlar UserRepositry'e eklendi. Artık burası doldurulacak.
 */