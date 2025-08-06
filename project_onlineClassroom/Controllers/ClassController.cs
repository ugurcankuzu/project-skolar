using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs;
using project_onlineClassroom.DTOs.ClassDTOs;
using project_onlineClassroom.DTOs.ParticipantDTOs;
using project_onlineClassroom.DTOs.TopicDTO;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Controllers
{
    [Route("classes")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly IPhotoService _photoService;

        public ClassController(IClassService classService, IPhotoService photoService) =>
            (_classService, _photoService) = (classService, photoService);

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<ClassDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetClassById(int id)
        {
            try
            {
                var @class = await _classService.GetClassByIdAsync(id, false, true);
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

        [HttpGet("{id}/participants")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<IEnumerable<ParticipantDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetParticipantsInClass(int id)
        {
            try
            {
                IEnumerable<Participant> participants = await _classService.GetParticipantInClassAsync(id);
                IEnumerable<ParticipantDTO> participantDTOs = participants.Select(p => new ParticipantDTO
                {
                    Id = p.UserId,
                    firstName = p.User.FirstName,
                    lastName = p.User.LastName,
                    email = p.User.Email,
                    JoinedAt = p.JoinedAt
                });

                return Ok(new GenericResponse<IEnumerable<ParticipantDTO>>(participantDTOs));
            }
            catch (ClassNotFoundException ex)
            {
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericResponse<object>(ex.Message));
            }
        }

        [HttpGet("{id}/topics")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<IEnumerable<TopicDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTopicsInClassAsync(int id)
        {
            try
            {
                IEnumerable<Topic> topics = await _classService.GetTopicsInClassAsync(id);
                IEnumerable<TopicDTO> topicDTOs = topics.Select(topic => new TopicDTO(topic));
                return Ok(new GenericResponse<IEnumerable<TopicDTO>>(topicDTOs));
            }
            catch (ClassNotFoundException ex)
            {
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericResponse<object>(ex.Message));
            }
        }

        [HttpGet("my-classes")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<List<UserClassSummaryDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status500InternalServerError)]
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
                    List<UserClassSummaryDTO> classDTOs = [.. classes.Select(c => new UserClassSummaryDTO(c))];
                    return Ok(new GenericResponse<List<UserClassSummaryDTO>>(classDTOs));
                }
                else
                {
                    List<Class> classes = await _classService.GetClassesByStudentIdAsync(id);
                    List<UserClassSummaryDTO> classDTOs = [.. classes.Select(c => new UserClassSummaryDTO(c))];
                    return Ok(new GenericResponse<List<UserClassSummaryDTO>>(classDTOs));
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

        [HttpPost("join")]
        [Authorize(Roles = "student")]
        [ProducesResponseType(typeof(GenericResponse<JoinClassResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> JoinClass([FromBody] JoinClassRequest req)
        {
            try
            {
                string joinCode = req.JoinCode;
                string? userId = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                {
                    throw new InvalidTokenException();
                }

                Participant participant = await _classService.JoinClassAsync(id, joinCode);
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
        /// <summary>
        /// Adds a student to a class by their email. Only the class owner can perform this action.
        /// </summary>
        /// <param name="classId">The ID of the class to add the student to.</param>
        /// <param name="studentEmail">The email of the student to add.</param>
        /// <returns>A success message if the user is added successfully.</returns>
        /// <remarks>
        /// This endpoint allows an educator to add a registered student to their class using the student's email address.
        /// 
        /// **Possible Error Responses:**
        /// - **400 Bad Request (`AlreadyParticipantException`):** The user is already a member of this class.
        /// - **401 Unauthorized (`InvalidTokenException`):** The JWT token is missing, invalid, or the user ID could not be extracted.
        /// - **403 Forbidden (`UnauthorizedAccessException`):** The current user is not the owner of the class.
        /// - **404 Not Found (`ClassNotFoundException`, `UserNotFoundException`):** The specified class or user could not be found.
        /// - **500 Internal Server Error:** An unexpected server error occurred.
        /// </remarks>
        [HttpPost("{classId}/add-user")]
        [Authorize(Roles = "educator")]
        [ProducesResponseType(typeof(GenericResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUserToClass([FromRoute] int classId, [FromBody] AddUserRequest req)
        {
            try
            {
                if (!ModelState.IsValid) throw new DataValidationException();
                string? educatorId = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(educatorId) || !int.TryParse(educatorId, out int id))
                {
                    throw new InvalidTokenException();
                }
                await _classService.AddStudentToClassByEmailAsync(classId, req.email, id);
                return Ok(new GenericResponse<string>(true, "User added to class successfully."));
            }
            catch (ClassNotFoundException ex)
            {
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (AlreadyParticipantException ex)
            {
                return BadRequest(new GenericResponse<object>(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
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
        [HttpPost("create")]
        [Authorize(Roles = "educator")]
        [ProducesResponseType(typeof(GenericResponse<ClassDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateClass([FromForm] CreateClassRequest req)
        {
            try
            {
                if (!ModelState.IsValid) throw new DataValidationException();

                string? userId = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                {
                    throw new InvalidTokenException();
                }
                if (req.ImageFile != null && req.ImageFile.Length > 0)
                {
                    var imageResult = await _photoService.AddPhotoAsync(req.ImageFile);
                    req.ImageUrl = imageResult?.url;
                    req.ImagePublicId = imageResult?.publicId;
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
