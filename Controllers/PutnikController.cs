using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using AvioIndustrija.Repository;
using AvioIndustrija.ViewModels;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System;
using AvioIndustrija.Utils;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace AvioIndustrija.Controllers
{
    [Authorize]
    public class PutnikController : Controller
    {
        private readonly IPutnikRepository _putnikRepository;
        private readonly IPhotoService _photoService;

        public PutnikController(IPutnikRepository putnikRepository, IPhotoService photoService)
        {
            this._putnikRepository = putnikRepository;
            this._photoService = photoService;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Putnik> putnici = await _putnikRepository.GetAll();
            return View(putnici);
        }
        public async Task<IActionResult> Details(int id)
        {
            Putnik putnik = await _putnikRepository.GetByIdAsyncNoTracking(id);
            return View(putnik);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Putnik putnik = await _putnikRepository.GetByIdAsyncNoTracking(id);
            return View(putnik);
        }
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePutnik(int id)
        {
            Putnik putnik = await _putnikRepository.GetByIdAsync(id);

            _putnikRepository.Delete(putnik);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePutnikViewModel putnikVM)
        {
            if (ModelState.IsValid)
            {
                var result = new ImageUploadResult();

                if (putnikVM.Image != null && putnikVM.Image.Length > 0)
                {
                    var fileExtension = Path.GetExtension(putnikVM.Image.FileName).ToLowerInvariant();
                    var validImageTypes = new[] { ".jpg", ".jpeg", ".png", ".gif", ".jfif" };

                    if (!validImageTypes.Contains(fileExtension))
                    {
                        ModelState.AddModelError("Image", "Please upload a valid image file (JPEG, PNG, GIF).");
                        return View(putnikVM);
                    }

                    using (var stream = new MemoryStream())
                    {
                        await putnikVM.Image.CopyToAsync(stream);
                        var img = Image.FromStream(stream);

                        // Parse crop data
                        dynamic cropData = Newtonsoft.Json.JsonConvert.DeserializeObject(putnikVM.CropData);
                        int x = cropData.x;
                        int y = cropData.y;
                        int width = cropData.width;
                        int height = cropData.height;

                        // Crop the image
                        var cropArea = new Rectangle(x, y, width, height);
                        var croppedImage = new Bitmap(cropArea.Width, cropArea.Height);

                        using (var graphics = Graphics.FromImage(croppedImage))
                        {
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.DrawImage(img, new Rectangle(0, 0, croppedImage.Width, croppedImage.Height), cropArea, GraphicsUnit.Pixel);
                        }

                        // Save the cropped image to a MemoryStream
                        using (var ms = new MemoryStream())
                        {
                            croppedImage.Save(ms, ImageFormat.Jpeg);
                            ms.Seek(0, SeekOrigin.Begin);

                            result = await _photoService.AddPhotoAsyncCropped(putnikVM.Image.FileName, ms, "Putnici");

                            ViewBag.ImagePath = result.SecureUrl;
                        }
                    }
                }

                var putnik = new Putnik
                {
                    Ime = putnikVM.Ime,
                    Prezime = putnikVM.Prezime,
                    Email = putnikVM.Email,
                    Pol = putnikVM.Pol,
                    ImageUrl = putnikVM.Image != null ? result.Url.ToString() : null
                };
                _putnikRepository.Add(putnik);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Kreiranje putnika nije uspjelo");
            }
            return View(putnikVM);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            Putnik putnik = await _putnikRepository.GetByIdAsyncNoTracking(id);
            if (putnik == null) return View("Error");
            var putnikVM = new EditPutnikViewModel
            {
                Ime = putnik.Ime,
                Prezime = putnik.Prezime,
                Email = putnik.Email,
                Pol = putnik.Pol,
                ExistingImageUrl = putnik.ImageUrl
            };

            return View(putnikVM);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditPutnikViewModel putnikVM)
        {
            var result = new ImageUploadResult();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Editovanje putnika nije uspjelo");
                return View("Edit", putnikVM);
            }

            var putnik = await _putnikRepository.GetByIdAsyncNoTracking(id);

            if (putnik != null)
            {
                if (putnikVM.Image != null && putnikVM.Image.Length > 0)
                {
                    if (!string.IsNullOrEmpty(putnik.ImageUrl))//putnik.ImageURL != null
                    {
                        var publicId = CloudinaryHelper.GetPublicIdFromUrlFromFolder(putnik.ImageUrl);

                        try
                        {
                            await _photoService.DeletePhotoAsync(publicId);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Could not delete photo");
                            return View(putnikVM);
                        }
                    }
                }

                if (putnikVM.Image != null && putnikVM.Image.Length > 0)
                {
                    var fileExtension = Path.GetExtension(putnikVM.Image.FileName).ToLowerInvariant();
                    var validImageTypes = new[] { ".jpg", ".jpeg", ".png", ".gif", ".jfif" };

                    if (!validImageTypes.Contains(fileExtension))
                    {
                        ModelState.AddModelError("Image", "Please upload a valid image file (JPEG, PNG, GIF).");
                        return View(putnikVM);
                    }

                    using (var stream = new MemoryStream())
                    {
                        await putnikVM.Image.CopyToAsync(stream);
                        var img = Image.FromStream(stream);

                        // Parse crop data
                        dynamic cropData = Newtonsoft.Json.JsonConvert.DeserializeObject(putnikVM.CropData);
                        int x = cropData.x;
                        int y = cropData.y;
                        int width = cropData.width;
                        int height = cropData.height;

                        // Crop the image
                        var cropArea = new Rectangle(x, y, width, height);
                        var croppedImage = new Bitmap(cropArea.Width, cropArea.Height);

                        using (var graphics = Graphics.FromImage(croppedImage))
                        {
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.DrawImage(img, new Rectangle(0, 0, croppedImage.Width, croppedImage.Height), cropArea, GraphicsUnit.Pixel);
                        }

                        // Save the cropped image to a MemoryStream
                        using (var ms = new MemoryStream())
                        {
                            croppedImage.Save(ms, ImageFormat.Jpeg);
                            ms.Seek(0, SeekOrigin.Begin);

                            result = await _photoService.AddPhotoAsyncCropped(putnikVM.Image.FileName, ms, "Putnici");

                            ViewBag.ImagePath = result.SecureUrl;
                        }
                    }
                }

                var putnikEdit = new Putnik
                {
                    PutnikId = id,
                    Ime = putnikVM.Ime,
                    Prezime = putnikVM.Prezime,
                    Email = putnikVM.Email,
                    Pol = putnikVM.Pol,
                    ImageUrl = putnikVM.Image != null ? result.Url.ToString() : putnik.ImageUrl
                };

                _putnikRepository.Update(putnikEdit);

                return RedirectToAction("Index");
            }
            else
            {
                return View(putnikVM);
            }
        }
    }
}
