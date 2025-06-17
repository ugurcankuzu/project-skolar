using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs.ClassDTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Controllers
{
    [Route("classes")]
    [ApiController]

    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        public ClassController(IClassService classService) => _classService = classService;

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetClassById(int id)
        {
            try
            {
                var @class = await _classService.GetClassByIdAsync(id, true, true);
                ClassDTO classDTO = new ClassDTO(@class);
                return Ok(new ClassResponse<ClassDTO>(classDTO));
            }
            catch (ClassNotFoundException ex)
            {
                return NotFound(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ClassResponse<Exception> { Success = false, Message = ex.Message });

            }
        }


        [HttpGet("my-classes")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        // Get classes of the user
        // Takes userId from Authentication token
        // Returns class information - For Educator: GetUserClassesEducatorResponse | For Student: GetUserClassesStudentResponse
        public async Task<IActionResult> GetUserClasses()
        {
            try
            {
                bool isEducator = User.IsInRole("educator");
                string? userId = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id)) throw new InvalidTokenException();

                if (isEducator)
                {
                    List<Class> classes = await _classService.GetClassesByEducatorIdAsync(id);
                    List<GetUserClassesEducatorResponse> classDTOs = [.. classes.Select(c => new GetUserClassesEducatorResponse(c))];
                    return Ok(new ClassResponse<List<GetUserClassesEducatorResponse>>(classDTOs));
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new ClassResponse<Exception>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new ClassResponse<Exception>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
        }
        [HttpPost("join/{classId}")]
        [Authorize(Roles = "student")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        // Join a class
        // Takes classId as a parameter
        // Takes userId from Authentication token
        // Returns participant information - JoinClassResponse
        public async Task<IActionResult> JoinClass([FromRoute] int classId)
        {
            try
            {
                string? userId = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                {
                    throw new InvalidTokenException();
                }
                Participant participant = await _classService.JoinClassAsync(id, classId);
                JoinClassResponse response = new JoinClassResponse(participant);
                return Ok(new ClassResponse<JoinClassResponse>(response));
            }
            catch (ClassNotFoundException ex)
            {
                return NotFound(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (RoleMismatchForThisActionException ex)
            {
                return BadRequest(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (AlreadyParticipantException ex)
            {
                return BadRequest(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (ClassFullException ex)
            {
                return BadRequest(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
        [HttpPost("create")]
        [Authorize(Roles = "educator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // Create a class
        // Takes userId from Authentication token
        // Takes class information from the request body
        // Returns class information - ClassDTO

        public async Task<IActionResult> CreateClass([FromBody] CreateClassRequest req)
        {
            try
            {
                //Data Annotation check
                if (!ModelState.IsValid) throw new DataValidationException();

                string? userId = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                {
                    throw new InvalidTokenException();
                }
                Class @class = await _classService.CreateClassAsync(req, id);
                ClassDTO classDTO = new ClassDTO(@class);
                return Ok(new ClassResponse<ClassDTO>(classDTO));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (DataValidationException ex)
            {
                return BadRequest(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
        [HttpDelete("{classId}/remove-user")]
        [Authorize(Roles = "educator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> RemoveUserFromClass([FromRoute] int classId, [FromQuery] int userId)
        {
            try
            {
                string? educatorId = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(educatorId) || !int.TryParse(educatorId, out int id))
                {
                    throw new InvalidTokenException();
                }
                Class @class = await _classService.RemoveParticipantFromClassAsync(userId, classId, id);
                ClassDTO classDTO = new ClassDTO(@class);
                return Ok(new ClassResponse<ClassDTO>(classDTO));
            }
            catch (RoleMismatchForThisActionException ex)
            {
                return BadRequest(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (NotParticipantException ex)
            {
                return BadRequest(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (ClassNotFoundException ex)
            {
                return NotFound(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
        [HttpDelete("{classId}/leave")]
        [Authorize(Roles = "student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> LeaveClass([FromRoute] int classId)
        {
            try
            {
                string? userId = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                {
                    throw new InvalidTokenException();
                }
                await _classService.LeaveClassAsync(id, classId);
                return Ok(new ClassResponse<string> { Success = true, Message = "Successfully left the class." });
            }
            catch (ClassNotFoundException ex)
            {
                return NotFound(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (RoleMismatchForThisActionException ex)
            {
                return BadRequest(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new ClassResponse<Exception> { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}

//TODO: DTO'lara ve validationlara bak.