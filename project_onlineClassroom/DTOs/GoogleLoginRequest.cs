using System.ComponentModel.DataAnnotations;

namespace project_onlineClassroom.DTOs
{
    public class GoogleLoginRequest
    {
        [Required(ErrorMessage = "Google ID token is required.")]
        public string IdToken { get; set; }

        public GoogleLoginRequest()
        {
        }
        public GoogleLoginRequest(string idToken)
        {
            IdToken = idToken;
        }
    }
}
