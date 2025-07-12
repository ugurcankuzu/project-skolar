using project_onlineClassroom.CustomError;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project_onlineClassroom.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IClassService _classService;
        public TopicService(ITopicRepository topicRepository, IClassService classService)
        {
            _topicRepository = topicRepository;
            _classService = classService;
        }

        /// <summary>
        /// Retrieves all topics for a specific class after verifying the class exists.
        /// </summary>
        /// <param name="classId">The unique ID of the class.</param>
        /// <returns>A <see cref="Task{List{Topic}}"/> containing all topics in the class. Returns an empty list if none exist.</returns>
        /// <exception cref="ClassNotFoundException">Propagated from the class service if the class with the specified ID does not exist.</exception>
        public async Task<List<Topic>> GetAllTopicsInClassAsync(int classId)
        {
            await _classService.GetClassByIdAsync(classId); // Validates class existence.
            return await _topicRepository.GetAllTopicsInClass(classId);
        }

        /// <summary>
        /// Retrieves a specific topic by its ID and the ID of the class it belongs to.
        /// </summary>
        /// <param name="topicId">The unique ID of the topic.</param>
        /// <param name="classId">The unique ID of the class.</param>
        /// <returns>The found <see cref="Topic"/> entity.</returns>
        /// <exception cref="NotFoundException">Propagated from the repository if no topic with the specified ID is found in the given class.</exception>
        public async Task<Topic> GetTopicByIdAsync(int topicId, int classId)
        {
            // The repository method efficiently handles both topic and class validation.
            return await _topicRepository.GetTopicByIdInClass(topicId, classId);
        }

        /// <summary>
        /// Creates a new topic within a specific class.
        /// Note: This method should be updated to accept a DTO and authorization details (e.g., current user ID).
        /// </summary>
        /// <param name="topic">The <see cref="Topic"/> entity to be created. Its ClassId property must be set.</param>
        /// <returns>The newly created <see cref="Topic"/> entity.</returns>
        /// <exception cref="ClassNotFoundException">Propagated from the class service if the class specified in the topic's ClassId does not exist.</exception>
        public async Task<Topic> AddTopic(Topic topic)
        {
            // Validate that the target class exists before creating a topic in it.
            await _classService.GetClassByIdAsync(topic.ClassId);
            return await _topicRepository.CreateTopic(topic);
        }

        /// <summary>
        /// Updates an existing topic's title and description.
        /// Note: This method should be updated to accept a DTO and authorization details.
        /// </summary>
        /// <param name="topicId">The ID of the topic to update. This parameter is used for retrieval but is not updated.</param>
        /// <param name="topic">A <see cref="Topic"/> object containing the new Title, Description, and the ClassId for verification.</param>
        /// <returns>The updated <see cref="Topic"/> entity.</returns>
        /// <exception cref="NotFoundException">Propagated from the repository if no topic with the specified ID is found in the given class.</exception>
        public async Task<Topic> UpdateTopicAsync(int topicId, Topic topic)
        {
            // "Get-Then-Update" pattern: First, retrieve the entity to ensure it exists and belongs to the correct class.
            Topic topicToUpdate = await _topicRepository.GetTopicByIdInClass(topicId, topic.ClassId);

            // Apply the updates from the input object.
            topicToUpdate.Title = topic.Title;
            topicToUpdate.Description = topic.Description;
            topicToUpdate.UpdatedAt = DateTime.UtcNow;

            // Save the updated entity.
            await _topicRepository.UpdateTopic(topicToUpdate);
            return topicToUpdate;
        }

        /// <summary>
        /// Deletes a topic from a class.
        /// Note: This method should be updated to include authorization checks.
        /// </summary>
        /// <param name="topicId">The unique ID of the topic to delete.</param>
        /// <param name="classId">The unique ID of the class from which the topic will be deleted.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Propagated from the repository if the topic to be deleted is not found in the given class.</exception>
        public async Task DeleteTopicAsync(int topicId, int classId)
        {
            // This "Get" acts as a guard clause to ensure the topic exists, providing a clear exception if not found.
            // A more performant approach would be to call a single Delete method in the repository and check the number of affected rows.
            Topic topic = await _topicRepository.GetTopicByIdInClass(topicId, classId);

            await _topicRepository.DeleteTopic(topic.Id, topic.ClassId);
        }
    }
}