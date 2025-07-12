using Microsoft.AspNetCore.Mvc;
using project_onlineClassroom.DTOs;
using project_onlineClassroom.DTOs.AuthDTOs;

namespace project_onlineClassroom.Controllers
{
    [ApiController]
    [Route("authtest")]
    public class AuthTest : ControllerBase
    {
        [HttpPost("test")]
        public IActionResult TestAuth([FromBody] GoogleLoginRequest req)
        {
            // This is a test endpoint to check if the controller is working
            return Ok(new LoginDTO(true, req.IdToken));
        }
    }
}


//DEBUG MODE'DA UYGULAMA PATLIYOR ANLAMADIĞIM ŞEKİLDE.