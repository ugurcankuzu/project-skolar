using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Util;
using ImageUploadResult = project_onlineClassroom.Interfaces.ImageUploadResult;

namespace project_onlineClassroom.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(CloudinarySettings settings)
        {
            var account = new Account
            {
                Cloud = settings.CloudName,
                ApiKey = settings.ApiKey,
                ApiSecret = settings.ApiSecret
            };
            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;
            await using var stream = file.OpenReadStream();
            var uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "onlineClassroom",
                PublicIdPrefix = "class_",
                Overwrite = true, // Overwrite existing files with the same public ID
                Transformation = new Transformation().FetchFormat("auto").Quality("auto")
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
            {
                throw new Exception($"Error uploading photo: {uploadResult.Error.Message}");
            }
            return new ImageUploadResult(uploadResult.PublicId, uploadResult.SecureUrl.ToString());
        }

        public Task DeletePhoto(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = _cloudinary.Destroy(deleteParams);
            return Task.CompletedTask;
        }
    }
}
