namespace project_onlineClassroom.DTOs
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsEducator { get; set; }
        public RegisterRequest(string email, string password, string confirmPassword, string firstName, string lastName, bool isEducator)
        {
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
            FirstName = firstName;
            LastName = lastName;
            IsEducator = isEducator;

        }
    }
}
