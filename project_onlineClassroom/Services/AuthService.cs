using System.Text.RegularExpressions;
using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;
using project_onlineClassroom.Util;

namespace project_onlineClassroom.Services
{
    public class AuthService: IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly JWTHelper _jwtHelper;

        public AuthService(IAuthRepository authRepository, IUserRepository userRepository, JWTHelper jwtHelper)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _authRepository.LoginAsync(request);
            return _jwtHelper.GenerateToken(user);

        }
        public async Task RegisterAsync(RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword) throw new PasswordMismatchException();
            Regex emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(request.Email)) throw new InvalidEmailException();
            if(await _userRepository.GetByEmailAsync(request.Email) != null) throw new UserExistsException();
            


        }
    }
}
