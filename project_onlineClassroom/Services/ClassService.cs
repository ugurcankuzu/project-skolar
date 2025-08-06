using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs.ClassDTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;
using System;
using System.Collections.Generic;
using System.Linq; // Added for .Any()
using System.Threading.Tasks;

namespace project_onlineClassroom.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IUserService _userService;
        private readonly IParticipantService _participantService;
        private readonly IJoinCodeGenerator _joinCodeGenerator;
        private readonly INotificationService _notificationService;
        private readonly ITopicService _topicService;

        public ClassService(IClassRepository classRepository, IUserService userService, IParticipantService participantService, IJoinCodeGenerator joinCodeGenerator, INotificationService notificationService, ITopicService topicService)
        {
            _classRepository = classRepository;
            _userService = userService;
            _participantService = participantService;
            _joinCodeGenerator = joinCodeGenerator;
            _notificationService = notificationService;
            _topicService = topicService;
        }

        /// <summary>
        /// Retrieves a class by its unique ID, with options to include related data.
        /// </summary>
        /// <param name="id">The unique ID of the class to retrieve.</param>
        /// <param name="includeParticipants">A boolean to indicate whether to load the participants of the class.</param>
        /// <param name="includeOwner">A boolean to indicate whether to load the owner of the class.</param>
        /// <returns>A <see cref="Task{Class}"/> representing the requested class.</returns>
        /// <exception cref="ClassNotFoundException">Propagated from the repository if no class with the specified ID is found.</exception>
        public async Task<Class> GetClassByIdAsync(int id, bool includeParticipants = false, bool includeOwner = false)
        {
            return await _classRepository.GetClassByIdAsync(id, includeParticipants, includeOwner);
        }

        /// <summary>
        /// Retrieves all classes owned by a specific educator.
        /// </summary>
        /// <param name="educatorId">The unique ID of the educator.</param>
        /// <returns>A <see cref="Task{List{Class}}"/> containing all classes owned by the educator. Returns an empty list if none are found.</returns>
        /// <exception cref="UserNotFoundException">Propagated from the user service if the educator with the specified ID does not exist.</exception>
        public async Task<List<Class>> GetClassesByEducatorIdAsync(int educatorId)
        {
            // First, validate that the educator exists.
            await _userService.GetUserProfileByIdAsync(educatorId);

            return await _classRepository.GetClassesByEducatorIdAsync(educatorId);
        }

        /// <summary>
        /// Allows a user to join a class, applying several business rules.
        /// </summary>
        /// <param name="userId">The ID of the user attempting to join.</param>
        /// <param name="classId">The ID of the class to be joined.</param>
        /// <param name="joinCode"> The unique join code for the class.</param>
        /// <returns>The created <see cref="Participant"/> record for the user in the class.</returns>
        /// <exception cref="ClassNotFoundException">Propagated from the repository if the specified class does not exist.</exception>
        /// <exception cref="UserNotFoundException">Propagated from the user service if the specified user does not exist.</exception>
        /// <exception cref="RoleMismatchForThisActionException">Thrown if an educator attempts to join a class as a participant.</exception>
        /// <exception cref="AlreadyParticipantException">Thrown if the user is already a participant in the class.</exception>
        /// <exception cref="ClassFullException">Thrown if the class has reached its maximum participant limit.</exception>
        public async Task<Participant> JoinClassAsync(int userId, string joinCode)
        {
            //Find class using join code
            Class @class = await _classRepository.GetClassByJoinCodeAsync(joinCode, includeParticipants: true);
            var user = await _userService.GetUserProfileByIdAsync(userId);

            if (user.IsEducator)
                throw new RoleMismatchForThisActionException("Educators cannot join a class as a participant.");

            // Using .Any() on the included participants is more efficient than a separate service call.
            if (@class.Participants.Any(p => p.UserId == userId))
                throw new AlreadyParticipantException("User is already a participant in this class.");

            if (@class.Participants.Count >= @class.UserLimit)
                throw new ClassFullException("The class has reached its user limit.");
            string cleanedJoinCode = joinCode.Trim();
            if (!String.Equals(@class.JoinCode, cleanedJoinCode, StringComparison.OrdinalIgnoreCase)) //Insensitive comparison for join code
                throw new ClassNotFoundException("Invalid join code for the class.");
            Participant createdParticipant = await _participantService.CreateParticipantAsync(userId, @class.Id);
            //Notification
            Notification notification = new Notification
            {
                UserId = @class.OwnerId, // Notify the class owner
                Message = $"{user.FirstName} {user.LastName} has joined your class '{@class.Title}'.",
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
            };
            try
            {
                await _notificationService.CreateNotificationAsync(notification);
            }
            catch (Exception ex)
            {
                // Log the error or handle it as needed
                Console.WriteLine($"Error creating notification: {ex.Message}");
            }
            // If all checks pass, create the participant.
            return createdParticipant;
        }

        /// <summary>
        /// Creates a new class for a specified educator.
        /// </summary>
        /// <param name="req">The DTO containing the details for the new class.</param>
        /// <param name="educatorId">The ID of the user who will own the new class.</param>
        /// <returns>The newly created <see cref="Class"/> entity.</returns>
        /// <exception cref="UserNotFoundException">Propagated from the user service if the educator with the specified ID does not exist.</exception>
        public async Task<Class> CreateClassAsync(CreateClassRequest req, int educatorId)
        {
            // First, validate that the educator exists.
            await _userService.GetUserProfileByIdAsync(educatorId);
            string joinCode = await _joinCodeGenerator.GenerateJoinCodeAsync();
            Class @class = new Class
            {
                Title = req.Title,
                Description = req.Description,
                ImageUrl = req.ImageUrl, // Optional, can be null
                ImagePublicId = req.ImagePublicId, // Optional, can be null
                UserLimit = req.UserLimit,
                OwnerId = educatorId,
                JoinCode = joinCode, // Unique join code for the class
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return await _classRepository.CreateClassAsync(@class);
        }

        /// <summary>
        /// Removes a participant from a class. This action can only be performed by the class owner.
        /// </summary>
        /// <param name="userIdToRemove">The ID of the user to be removed from the class.</param>
        /// <param name="classId">The ID of the class.</param>
        /// <param name="currentEducatorId">The ID of the user performing the action, who must be the class owner.</param>
        /// <returns>The updated <see cref="Class"/> entity after removing the participant.</returns>
        /// <exception cref="ClassNotFoundException">Propagated from the repository if the specified class does not exist.</exception>
        /// <exception cref="UserNotFoundException">Propagated from the user service if the educator performing the action does not exist.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if the user performing the action is not the owner of the class.</exception>
        /// <exception cref="NotParticipantException">Thrown if the user to be removed is not a participant in the class.</exception>
        public async Task<Class> RemoveParticipantFromClassAsync(int userIdToRemove, int classId, int currentEducatorId)
        {
            var @class = await _classRepository.GetClassByIdAsync(classId);
            await _userService.GetUserProfileByIdAsync(currentEducatorId); // Validate the educator exists

            // Authorization check: Only the owner can remove participants.
            if (@class.OwnerId != currentEducatorId)
                throw new UnauthorizedAccessException("Only the class owner can remove a participant.");

            // Use the participant service to find if the user is a participant.
            var existingParticipant = await _participantService.FindParticipantByUserIdAsync(userIdToRemove, @class.Id)
                ?? throw new NotParticipantException("The specified user is not a participant in this class.");

            await _participantService.DeleteParticipantFromClassByUserIdAsync(existingParticipant.UserId, @class.Id);

            // Return the updated class state with participants.
            return await _classRepository.GetClassByIdAsync(@class.Id, includeParticipants: true);
        }

        /// <summary>
        /// Allows a student to leave a class they are participating in.
        /// </summary>
        /// <param name="userId">The ID of the user leaving the class.</param>
        /// <param name="classId">The ID of the class to leave.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ClassNotFoundException">Propagated from the repository if the specified class does not exist.</exception>
        /// <exception cref="UserNotFoundException">Propagated from the user service if the specified user does not exist.</exception>
        /// <exception cref="RoleMismatchForThisActionException">Thrown if an educator attempts to leave a class.</exception>
        /// <exception cref="NotParticipantException">Thrown if the user is not a participant in the class.</exception>
        public async Task LeaveClassAsync(int userId, int classId)
        {
            // These calls validate the existence of both the class and the user.
            await _classRepository.GetClassByIdAsync(classId);
            var user = await _userService.GetUserProfileByIdAsync(userId);

            if (user.IsEducator)
                throw new RoleMismatchForThisActionException("Educators cannot use this function to leave a class.");

            var existingParticipant = await _participantService.FindParticipantByUserIdAsync(user.Id, classId)
                ?? throw new NotParticipantException("You are not a participant in this class.");

            await _participantService.DeleteParticipantFromClassByUserIdAsync(existingParticipant.UserId, classId);
        }

        public async Task<int> GetClassCountByEducatorIdAsync(int educatorId)
        {
            User user = await _userService.GetUserProfileByIdAsync(educatorId);
            if (!user.IsEducator)
            {
                throw new CustomError.RoleMismatchForThisActionException("Only educators can have classes.");
            }
            return await _classRepository.GetNumberOfClassesByEducatorIdAsync(educatorId);
        }
        /// <summary>
        /// Returns the number of classes a student is enrolled in by their user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="RoleMismatchForThisActionException"></exception>
        public async Task<int> GetClassCountByStudentIdAsync(int userId)
        {
            User user = await _userService.GetUserProfileByIdAsync(userId);
            if (user.IsEducator)
            {
                throw new CustomError.RoleMismatchForThisActionException("Educators cannot have classes as students.");
            }
            return await _classRepository.GetNumberOfClassesByStudentIdAsync(userId);
        }

        public async Task<List<Class>> GetClassesByStudentIdAsync(int id)
        {
            await _userService.GetUserProfileByIdAsync(id); // Validate user exists
            List<Class> classes = await _classRepository.GetClassesByStudentIdAsync(id);
            return classes;
        }

        public async Task<IEnumerable<Participant>> GetParticipantInClassAsync(int classId)
        {
            // Validate that the class exists before fetching participants.
            await _classRepository.GetClassByIdAsync(classId);
            // Fetch and return the participants for the specified class.
            return await _participantService.GetParticipantsByClassIdAsync(classId);

        }

        public async Task<IEnumerable<Topic>> GetTopicsInClassAsync(int classId)
        {
            // Validate that the class exists before fetching topics.
            await _classRepository.GetClassByIdAsync(classId);
            // Fetch and return the topics for the specified class.
            IEnumerable<Topic> topics = await _topicService.GetAllTopicsInClassAsync(classId, true);
            return topics;
        }

        public async Task AddStudentToClassByEmailAsync(int classId, string email, int currentUserId)
        {
            //Validate class exists and current user is educator of this class
            Class @class = await _classRepository.GetClassByIdAsync(classId, includeParticipants: true, includeOwner: true);
            if (@class == null)
            {
                throw new ClassNotFoundException("Class not found.");
            }
            if (@class.OwnerId != currentUserId)
            {
                throw new UnauthorizedAccessException("Only the class owner can add students by email.");
            }
            //Validate User exists with this email -- If not found it throws error inside userRepository
            User user = await _userService.GetUserProfileByEmail(email);

            // Prevent adding an educator as a student
            if (user.IsEducator)
            {
                throw new RoleMismatchForThisActionException("An educator cannot be added as a student to a class.");
            }

            //Already participant check
            bool isAlreadyParticipant = await _participantService.IsUserParticipantInClassAsync(user.Id, classId);
            if (isAlreadyParticipant)
            {
                throw new AlreadyParticipantException("User is already a participant in this class.");
            }
            //Create participant
            Participant participant = await _participantService.CreateParticipantAsync(user.Id, classId);
            //Notification
            try
            {
                Notification studentNotification = new Notification
                {
                    UserId = user.Id, // Notify the student
                    Message = $"You have been added to the class '{@class.Title}' by {@class.Owner.FirstName} {@class.Owner.LastName}.",
                    Link = $"/skolar/classroom",
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false,
                };
                await _notificationService.CreateNotificationAsync(studentNotification);
            }
            catch (Exception ex) 
            {
                // Log the error or handle it as needed
                Console.WriteLine($"Error creating notification for student: {ex.Message}");
            }


        }
    }
}