using System.ComponentModel.DataAnnotations;

namespace project_onlineClassroom.DTOs.UserDTOs
{
    public class RoleSelectRequest
    {
        [Required(ErrorMessage = "Your new role is required.")]
        public bool IsEducator { get; set; }
        public RoleSelectRequest() { }
        public RoleSelectRequest(bool isEducator)
        {
            IsEducator = isEducator;
        }

    }
}
