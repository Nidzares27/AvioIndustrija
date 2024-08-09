using AvioIndustrija.Helpers;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AvioIndustrija.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file, string folder)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = folder
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<ImageUploadResult> AddPhotoAsyncCropped(string fileName, Stream stream, string folder)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, stream),
                Folder = folder
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult;

        }
        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }

        [HttpPost]
        public async Task<string> UploadImage(string filePath)
        {
            var uploadResult = await UploadToCloudinary(filePath);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.IO.File.Delete(filePath);
                return uploadResult.Url.ToString();
            }
            else
            {
                return "";
            }
        }

        public async Task<ImageUploadResult> UploadToCloudinary(string filePath)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filePath),
                Folder = "Avioni"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult;
        }

    }
}
