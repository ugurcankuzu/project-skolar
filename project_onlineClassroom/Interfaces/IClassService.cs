using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs.ClassDTOs;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Interfaces
{
    public interface IClassService
    {
        /// <summary>
        /// Retrieves a class by its ID. If the class does not exist, throws a <see cref="ClassNotFoundException"/>.
        /// </summary>
        /// <returns><see cref="Task{Class}"/></returns>
        /// <exception cref="ClassNotFoundException"></exception>
        /// <param name="id"> The ID of the class to retrieve.</param>
        /// <param name="includeOwner"> Whether to include the class owner in the result.</param>
        /// <param name="includeParticipants"> Whether to include the participants in the result.</param>
        Task<Class> GetClassByIdAsync(int id, bool includeParticipants = false, bool includeOwner = false);
        /// <summary>
        /// Returns a list of educator's classes using educator's ID. If the educator does not exist, throws a <see cref="UserNotFoundException"/>.
        /// </summary>
        /// <returns><see cref="List{Class}"/></returns>
        /// <exception cref="UserNotFoundException"></exception>
        /// <param name="educatorId"> The ID of the educator whose classes are to be retrieved.</param>
        Task<List<Class>> GetClassesByEducatorIdAsync(int educatorId);
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
        Task<Participant> JoinClassAsync(int userId, int classId);
        /// <summary>
        /// Creates a new class with the provided details. If the educator does not exist, throws a <see cref="UserNotFoundException"/>.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="educatorId"></param>
        /// <returns><see cref="Task{Class}"/></returns>
        /// <exception cref="UserNotFoundException"></exception>
        Task<Class> CreateClassAsync(CreateClassRequest req, int educatorId);
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
        Task<Class> RemoveParticipantFromClassAsync(int userId, int classId, int educatorId);
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
        Task LeaveClassAsync(int userId, int classId);
        /// <summary>
        /// Retrieves the count of classes associated with a specific educator by their ID. If the educator does not exist, throws a <see cref="UserNotFoundException"/>.
        /// </summary>
        /// <param name="educatorId"></param>
        /// <returns></returns>
        Task<int> GetClassCountByEducatorIdAsync(int educatorId);
        Task<int> GetClassCountByStudentIdAsync(int userId);
        Task<List<Class>> GetClassesByStudentIdAsync(int id);
    }
}

//TODO:
/*
    1-> GetClassesByStudentIdAsync
    2-> DeleteClassAsync
    3-> UpdateClassAsync
    4-> LeaveClassAsync
 */