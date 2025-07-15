using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs;
using project_onlineClassroom.DTOs.SummaryDTOs;
using project_onlineClassroom.Interfaces;

namespace project_onlineClassroom.Controllers
{
    public class SummaryController : ControllerBase
    {
        private readonly ISummaryService _summaryService;
        public SummaryController(ISummaryService summaryService)
        {
            _summaryService = summaryService;
        }

        [HttpGet("dashboard/summary/educator")]
        [Authorize(Roles = "educator")]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDashboardSummary()
        {
            try
            {
                string? userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                {
                    throw new InvalidTokenException();
                }
                SummaryEducatorDTO summary = await _summaryService.GetDashboardSummaryEducatorAsync(id);
                return Ok(new GenericResponse<SummaryEducatorDTO>(summary));
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new GenericResponse<SummaryEducatorDTO>(false, ex.Message));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new GenericResponse<SummaryEducatorDTO>(false, ex.Message));
            }
            catch (RoleMismatchForThisActionException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
        [HttpGet("dashboard/summary/student")]
        [Authorize(Roles = "student")]
        [ProducesResponseType(typeof(GenericResponse<SummaryStudentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDashboardSummaryForStudent()
        {

            try
            {
                string? userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                {
                    throw new InvalidTokenException();
                }
                SummaryStudentDTO summary = await _summaryService.GetDashboardSummaryStudentAsync(id);
                return Ok(new GenericResponse<SummaryStudentDTO>(summary));
            }

            catch (InvalidTokenException ex)
            {
                return Unauthorized(new GenericResponse<SummaryStudentDTO>(false, ex.Message));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new GenericResponse<SummaryStudentDTO>(false, ex.Message));
            }
            catch (RoleMismatchForThisActionException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
// Other methods can be added here as needed

