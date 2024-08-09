using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace AvioIndustrija.Utils
{
    public class UploadImageFromExcel
    {
        private readonly HttpClient _httpClient;

        public UploadImageFromExcel(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        [HttpPost]
        public async Task<string> DownloadImage([FromBody] string imageUrl)
        {
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                return "";
            }

            try
            {
                var filePath = await DownloadImageAsync(imageUrl);
                return filePath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private async Task<string> DownloadImageAsync(string imageUrl)
        {
            // Fetch the image from the URL
            var response = await _httpClient.GetAsync(imageUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to download image: {response.ReasonPhrase}");
            }

            // Read the image content
            var imageBytes = await response.Content.ReadAsByteArrayAsync();

            // Determine the file extension
            var fileExtension = GetFileExtensionFromContentType(response.Content.Headers.ContentType.MediaType);

            // Generate a unique file name
            var fileName = $"{Guid.NewGuid()}{fileExtension}";

            // Define the path where the file will be saved
            var filePath = Path.Combine("wwwroot/images", fileName);

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // Save the image to the file
            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

            return filePath;
        }

        private string GetFileExtensionFromContentType(string contentType)
        {
            return contentType switch
            {
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                "image/gif" => ".gif",
                "image/bmp" => ".bmp",
                "image/tiff" => ".tiff",
                _ => throw new NotSupportedException($"Content type {contentType} is not supported.")
            };
        }
    }
}
