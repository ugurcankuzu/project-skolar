
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_onlineClassroom.CustomError;
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
        [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                string? userId = User.FindFirstValue("id");
                if (String.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id)) throw new InvalidTokenException();
                User user = await _userService.GetUserProfileByIdAsync(id);
                return Ok(new ProfileResponse(new ProfileDTO(user)));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new ProfileResponse(false, ex.Message));
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new ProfileResponse(false, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProfileResponse(false, ex.Message));
            }
        }

        [HttpPatch("me")]
        [Authorize]
        [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDTO updateRequest)
        {
            try
            {
                string? userId = User.FindFirstValue("id");
                if (String.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id)) throw new InvalidTokenException();
                User user = await _userService.UpdateUserProfileAsync(id, updateRequest);
                return Ok(new ProfileResponse(new ProfileDTO(user)));

            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new ProfileResponse(false, ex.Message));
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new ProfileResponse(false, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProfileResponse(false, ex.Message));
            }
        }
        [HttpDelete("me")]
        [Authorize]
        [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                string? userId = User.FindFirstValue("id");
                if (String.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id)) throw new InvalidTokenException();
                await _userService.DeleteUser(id);
                return Ok(new ProfileResponse(true, "User deleted successfully."));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new ProfileResponse(false, ex.Message));
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new ProfileResponse(false, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProfileResponse(false, ex.Message));
            }
        }
    }
}

/*
    TODO: Custom Errorlar UserRepositry'e eklendi. Artık burası doldurulacak.
 */