namespace project_onlineClassroom.Interfaces
{
    public record ImageUploadResult(string publicId, string url);

    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task DeletePhoto(string publicId);
    }
}
