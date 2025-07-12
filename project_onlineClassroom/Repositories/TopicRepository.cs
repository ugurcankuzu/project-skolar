using Microsoft.EntityFrameworkCore;
using project_onlineClassroom.CustomError; // Assuming NotFoundException is in this namespace
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project_onlineClassroom.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly AppDbContext _context;
        public TopicRepository(AppDbContext context) => _context = context;

        /// <summary>
        /// Retrieves a list of all topics associated with a specific class.
        /// Returns an empty list if the class has no topics.
        /// </summary>
        /// <param name="classId">The unique ID of the class.</param>
        /// <returns>A <see cref="Task{List{Topic}}"/> containing the topics of the class.</returns>
        public async Task<List<Topic>> GetAllTopicsInClass(int classId)
        {
            return await _context.Topics
                .Where(t => t.ClassId == classId)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific topic by its ID, ensuring it belongs to the specified class.
        /// </summary>
        /// <param name="topicId">The unique ID of the topic to retrieve.</param>
        /// <param name="classId">The unique ID of the class that the topic must belong to.</param>
        /// <returns>The found <see cref="Topic"/> entity.</returns>
        /// <exception cref="Exception">Thrown if no topic with the specified ID is found in the given class.</exception>
        public async Task<Topic> GetTopicByIdInClass(int topicId, int classId)
        {
            // Using a more specific exception is recommended.
            Topic topic = await _context.Topics
                .Where(t => t.Id == topicId && t.ClassId == classId)
                .FirstOrDefaultAsync()
                ?? throw new Exception($"Topic with ID {topicId} not found in class {classId}.");
            return topic;
        }

        /// <summary>
        /// Adds a new topic to the database and saves the changes.
        /// </summary>
        /// <param name="topic">The <see cref="Topic"/> entity to be created.</param>
        /// <returns>The created <see cref="Topic"/> entity with its new database-generated ID.</returns>
        public async Task<Topic> CreateTopic(Topic topic)
        {
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();
            return topic;
        }

        /// <summary>
        /// Updates an existing topic in the database with the provided entity state and saves the changes.
        /// This method assumes the entity has been retrieved and modified in the service layer.
        /// </summary>
        /// <param name="topic">The <see cref="Topic"/> entity with updated values.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task UpdateTopic(Topic topic)
        {
            _context.Topics.Update(topic);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a topic from the database in a single operation, ensuring it belongs to the specified class.
        /// </summary>
        /// <param name="topicId">The unique ID of the topic to delete.</param>
        /// <param name="classId">The unique ID of the class from which the topic will be deleted.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task DeleteTopic(int topicId, int classId)
        {
            await _context.Topics
                .Where(t => t.Id == topicId && t.ClassId == classId)
                .ExecuteDeleteAsync();
        }
    }
}