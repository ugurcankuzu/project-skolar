using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs;
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
        [ProducesResponseType(typeof(GenericResponse<ClassDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetClassById(int id)
        {
            try
            {
                var @class = await _classService.GetClassByIdAsync(id, true, true);
                ClassDTO classDTO = new ClassDTO(@class);
                return Ok(new GenericResponse<ClassDTO>(classDTO));
            }
            catch (ClassNotFoundException ex)
            {
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse<object>(ex.Message));

            }
        }


        [HttpGet("my-classes")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<List<GetUserClassesEducatorResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status500InternalServerError)]

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
                    return Ok(new GenericResponse<List<GetUserClassesEducatorResponse>>(classDTOs));
                }
                else
                {
                    throw new NotImplementedException();
                }
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
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericResponse<object>(ex.Message));
            }
        }
        [HttpPost("join/{classId}")]
        [Authorize(Roles = "student")]
        [ProducesResponseType(typeof(GenericResponse<JoinClassResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status401Unauthorized)]

        // Join a class
        // Takes classId as a parameter
        // Takes userId from Authentication token
        // Returns participant information - JoinGenericResponse
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
                return Ok(new GenericResponse<JoinClassResponse>(response));
            }
            catch (ClassNotFoundException ex)
            {
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (RoleMismatchForThisActionException ex)
            {
                return BadRequest(new GenericResponse<object>(ex.Message));
            }
            catch (AlreadyParticipantException ex)
            {
                return BadRequest(new GenericResponse<object>(ex.Message));
            }
            catch (ClassFullException ex)
            {
                return BadRequest(new GenericResponse<object>(ex.Message));
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new GenericResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
        [HttpPost("create")]
        [Authorize(Roles = "educator")]
        [ProducesResponseType(typeof(GenericResponse<ClassDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status401Unauthorized)]
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
                return Ok(new GenericResponse<ClassDTO>(classDTO));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new GenericResponse<object>(ex.Message));
            }
            catch (DataValidationException ex)
            {
                return BadRequest(new GenericResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
        [HttpDelete("{classId}/remove-user")]
        [Authorize(Roles = "educator")]
        [ProducesResponseType(typeof(GenericResponse<ClassDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status401Unauthorized)]

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
                return Ok(new GenericResponse<ClassDTO>(classDTO));
            }
            catch (RoleMismatchForThisActionException ex)
            {
                return BadRequest(new GenericResponse<object>(ex.Message));
            }
            catch (NotParticipantException ex)
            {
                return BadRequest(new GenericResponse<object>(ex.Message));
            }
            catch (ClassNotFoundException ex)
            {
                return NotFound(new GenericResponse<object>(ex.Message));
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
        [HttpDelete("{classId}/leave")]
        [Authorize(Roles = "student")]
        [ProducesResponseType(typeof(GenericResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status401Unauthorized)]

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
                return Ok(new GenericResponse<string>(true, "Successfully leave from class"));
            }
            catch (ClassNotFoundException ex)
            {
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (RoleMismatchForThisActionException ex)
            {
                return BadRequest(new GenericResponse<object>(ex.Message));
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new GenericResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}

//TODO: DTO'lara ve validationlara bak.