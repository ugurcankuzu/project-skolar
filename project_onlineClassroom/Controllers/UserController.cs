
using Microsoft.AspNetCore.Mvc;
using project_onlineClassroom.Models;
using project_onlineClassroom.Repositories;

namespace project_onlineClassroom.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        public UserController(UserRepository userRepository) : base() => _userRepository = userRepository;

        // GET: User
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                List<User> users = await _userRepository.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching users: {ex.Message}");
            }
        }
    }
}

/*
    TODO: Custom Errorlar UserRepositry'e eklendi. Artık burası doldurulacak.
 */