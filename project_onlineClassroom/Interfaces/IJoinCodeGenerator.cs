namespace project_onlineClassroom.Interfaces
{
    public interface IJoinCodeGenerator
    {
        /// <summary>
        /// Generates a unique join code for a class.
        /// </summary>
        /// <returns>A unique join code as a string.</returns>
        Task<string> GenerateJoinCodeAsync();
    }
}
