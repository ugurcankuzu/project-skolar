using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs.ClassDTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IUserService _userService;
        private readonly IParticipantService _participantService;
        public ClassService(IClassRepository classRepository, IUserService userService, IParticipantService participantService)
        {
            _classRepository = classRepository;
            _userService = userService;
            _participantService = participantService;
        }
        /// <summary>
        /// Checks if a user exists by their ID. If exists returns the user entity. If not exists, throws an <see cref="UserNotFoundException"/>
        /// </summary>
        /// <exception cref="UserNotFoundException"></exception>
        /// <returns><see cref="Task{User}"/></returns>
        /// <param name="userId"> The ID of the user to check.</param>
        private async Task<User> IsUserExists(int userId)
        {
            User user = await _userService.GetUserProfileByIdAsync(userId);
            return user;
        }
        /// <summary>
        /// Checks if a class exists by its ID. If exists returns the class entity. If not exists, throws an <see cref="ClassNotFoundException"/>
        /// </summary>
        /// <returns><see cref="Task{Class}"/></returns>
        /// <exception cref="ClassNotFoundException"></exception>
        /// <param name="classId"> The ID of the class to check.</param>
        /// <param name="includeParticipants"> Whether to include participants in the class result.</param>
        /// <param name="includeOwner"> Whether to include the class owner in the result.</param>
        private async Task<Class> IsClassExists(int classId, bool includeParticipants = false, bool includeOwner = false)
        {
            // Check if the class exists
            Class? @class = await _classRepository.GetClassByIdAsync(classId, includeParticipants, includeOwner) ?? throw new ClassNotFoundException();
            return @class;
        }
        /// <summary>
        /// Retrieves a class by its ID. If the class does not exist, throws a <see cref="ClassNotFoundException"/>.
        /// </summary>
        /// <returns><see cref="Task{Class}"/></returns>
        /// <exception cref="ClassNotFoundException"></exception>
        /// <param name="id"> The ID of the class to retrieve.</param>
        /// <param name="includeOwner"> Whether to include the class owner in the result.</param>
        /// <param name="includeParticipants"> Whether to include the participants in the result.</param>
        public async Task<Class> GetClassByIdAsync(int id, bool includeParticipants = false, bool includeOwner = false)
        {
            // Check if the class exists
            Class existingClass = await IsClassExists(id, includeParticipants, includeOwner);
            return existingClass;
        }
        /// <summary>
        /// Returns a list of educator's classes using educator's ID. If the educator does not exist, throws a <see cref="UserNotFoundException"/>.
        /// </summary>
        /// <returns><see cref="List{Class}"/></returns>
        /// <exception cref="UserNotFoundException"></exception>
        /// <param name="educatorId"> The ID of the educator whose classes are to be retrieved.</param>
        public async Task<List<Class>> GetClassesByEducatorIdAsync(int educatorId)
        {

            // Check if the educator exists
            User educator = await IsUserExists(educatorId);
            // Get the classes taught by the educator
            List<Class> classes = await _classRepository.GetClassesByEducatorIdAsync(educator.Id);
            return classes;
        }
        /// <summary>
        /// User joins a class by providing their user ID and the class ID. If the class does not exist,
        /// throws a <see cref="ClassNotFoundException"/>. If the user does not exist, throws an <see cref="UserNotFoundException"/>. 
        /// If the user is an educator, throws a <see cref="RoleMismatchForThisActionException"/>. If the user is already a participant in the class, throws an <see cref="AlreadyParticipantException"/>.
        /// If the class is full, throws a <see cref="ClassFullException"/>.
        /// </summary>
        /// <returns><see cref="Task{Participant}"/></returns>
        /// <exception cref="ClassNotFoundException"></exception>
        /// <exception cref="UserNotFoundException"></exception>
        /// <exception cref="RoleMismatchForThisActionException"></exception>
        /// <exception cref="AlreadyParticipantException"></exception>
        /// <exception cref="ClassFullException"></exception>
        /// <param name="userId"> The ID of the user who wants to join the class.</param>
        /// <param name="classId"> The ID of the class to join.</param>
        public async Task<Participant> JoinClassAsync(int userId, int classId)
        {
            // Check if the class exists
            Class @class = await IsClassExists(classId, true, true);
            // Check if the user exists
            User user = await IsUserExists(userId);
            // Check if the user is an educator
            if (user.IsEducator) throw new RoleMismatchForThisActionException("Educators cannot participate a class.");

            // Check if the user is already a participant  
            Participant? existingParticipant = await _participantService.GetParticipantInClassByUserIdAsync(userId, classId);
            if (existingParticipant != null) throw new AlreadyParticipantException();

            if (@class.Participants.Count >= @class.UserLimit) throw new ClassFullException();

            Participant newParticipant = await _participantService.CreateParticipantAsync(userId, classId);
            return newParticipant;
        }
        /// <summary>
        /// Creates a new class with the provided details. If the educator does not exist, throws a <see cref="UserNotFoundException"/>.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="educatorId"></param>
        /// <returns><see cref="Task{Class}"/></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<Class> CreateClassAsync(CreateClassRequest req, int educatorId)
        {
            User educator = await IsUserExists(educatorId);
            Class @class = new Class
            {
                Title = req.Title,
                UserLimit = req.UserLimit,
                OwnerId = educatorId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            Class newClass = await _classRepository.CreateClassAsync(@class);
            return newClass;
        }
        /// <summary>
        /// Removes a participant from a class. The user must be an educator and the class owner to perform this action. If the user is not an educator, throws a <see cref="RoleMismatchForThisActionException"/>.
        /// If the class does not exist, throws a <see cref="ClassNotFoundException"/>. If the user is not a participant in the class, throws a <see cref="NotParticipantException"/>.
        /// If the class owner is not the same as the educator, throws a <see cref="RoleMismatchForThisActionException"/>.
        /// If the user is not found, throws a <see cref="UserNotFoundException"/>.
        /// If the class is not found after removing the participant, throws a <see cref="ClassNotFoundException"/>.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="classId"></param>
        /// <param name="educatorId"></param>
        /// <returns></returns>
        /// <exception cref="RoleMismatchForThisActionException"></exception>
        /// <exception cref="NotParticipantException"></exception>
        /// <exception cref="ClassNotFoundException"></exception>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<Class> RemoveParticipantFromClassAsync(int userId, int classId, int educatorId)
        {
            // Check if the Class exists
            Class @class = await IsClassExists(classId);
            // Check if the user exists
            User educator = await IsUserExists(educatorId);
            // Check if the user is an educator
            if (!educator.IsEducator) throw new RoleMismatchForThisActionException("Students cannot remove a participant from class");
            // Check if the class owner is the same as the educator
            if (@class.OwnerId != educator.Id) throw new RoleMismatchForThisActionException("Only class owner can remove a participant from class.");
            // Check if the user is a participant
            Participant? existingParticipant = await _participantService.GetParticipantInClassByUserIdAsync(userId, @class.Id) ?? throw new NotParticipantException();
            await _participantService.DeleteParticipantFromClassByUserIdAsync(existingParticipant.UserId, @class.Id);
            Class updatedClass = await _classRepository.GetClassByIdAsync(@class.Id, true) ?? throw new ClassNotFoundException("Class not found after removing participant.");
            return updatedClass;
        }
        /// <summary>
        /// Allows a user to leave a class. The user must not be an educator to perform this action. If the class does not exist, throws a <see cref="ClassNotFoundException"/>.
        /// If the user does not exist, throws a <see cref="UserNotFoundException"/>. If the user is an educator, throws a <see cref="RoleMismatchForThisActionException"/>.
        /// If the user is not a participant in the class, throws a <see cref="NotParticipantException"/>.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        /// <exception cref="RoleMismatchForThisActionException"></exception>
        /// <exception cref="NotParticipantException"></exception>
        /// <exception cref="ClassNotFoundException"></exception>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task LeaveClassAsync(int userId, int classId)
        {
            // Check if the class exists
            Class @class = await IsClassExists(classId);
            // Check if the user exists
            User user = await IsUserExists(userId);
            // Check if the user is an educator
            if (user.IsEducator) throw new RoleMismatchForThisActionException("Educators cannot leave a class.");
            // Check if the user is a participant  
            Participant? existingParticipant = await _participantService.GetParticipantInClassByUserIdAsync(user.Id, @class.Id) ?? throw new NotParticipantException();
            await _participantService.DeleteParticipantFromClassByUserIdAsync(existingParticipant.UserId, @class.Id);
        }
    }
}
