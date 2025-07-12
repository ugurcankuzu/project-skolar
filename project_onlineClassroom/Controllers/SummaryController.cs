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

        [HttpGet("dashboard/summary")]
        [Authorize(Roles = "educator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDashboardSummary()
        {
            try
            {
                string? userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                {
                    throw new InvalidTokenException();
                }
                SummaryEducatorDTO summary = await _summaryService.GetDashboardSummaryAsync(id);
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
    }
}
// Other methods can be added here as needed

