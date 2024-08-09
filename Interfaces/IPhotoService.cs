using CloudinaryDotNet.Actions;
namespace AvioIndustrija.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file, string folder);
        Task<ImageUploadResult> AddPhotoAsyncCropped(string fileName, Stream stream, string folder);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
        Task<string> UploadImage(string filePath);
        Task<ImageUploadResult> UploadToCloudinary(string filePath);

    }
}
