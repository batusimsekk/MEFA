using Microsoft.AspNetCore.Mvc;
using MefaOzelEgitim.Domain.Entities;
using MefaOzelEgitim.Domain.Interfaces;
using MefaOzelEgitim.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;

namespace MefaOzelEgitim.Web.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AnnouncementsController : Controller
    {
        private readonly IRepository<Announcement> _announcementRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AnnouncementsController(IRepository<Announcement> announcementRepository, IWebHostEnvironment webHostEnvironment)
        {
            _announcementRepository = announcementRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var announcements = await _announcementRepository.GetAllAsync();
            var orderedAnnouncements = announcements.OrderByDescending(x => x.CreatedDate).ToList();
            return View(orderedAnnouncements);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new AnnouncementCreateViewModel
            {
                PublishDate = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnnouncementCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imageUrl = string.Empty;

                // Görseli wwwroot/images/announcements klasörüne kaydetme
                if (model.Image != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "announcements");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(fileStream);
                    }

                    imageUrl = "/images/announcements/" + uniqueFileName;
                }

                var announcement = new Announcement
                {
                    Title = model.Title,
                    Summary = model.Summary,
                    Content = model.Content,
                    ImageUrl = imageUrl,
                    PublishDate = model.PublishDate,
                    Slug = model.Title.ToLower()
                        .Replace(" ", "-")
                        .Replace("ç", "c")
                        .Replace("ğ", "g")
                        .Replace("ı", "i")
                        .Replace("ö", "o")
                        .Replace("ş", "s")
                        .Replace("ü", "u"),
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                await _announcementRepository.AddAsync(announcement);
                await _announcementRepository.SaveChangesAsync();

                TempData["SuccessMessage"] = "Duyuru başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Lütfen tüm zorunlu alanları doldurun.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var announcement = await _announcementRepository.GetByIdAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            var model = new AnnouncementEditViewModel
            {
                Id = announcement.Id,
                Title = announcement.Title,
                Summary = announcement.Summary,
                Content = announcement.Content,
                ExistingImageUrl = announcement.ImageUrl,
                PublishDate = announcement.PublishDate
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AnnouncementEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var announcement = await _announcementRepository.GetByIdAsync(model.Id);
                if (announcement == null)
                {
                    return NotFound();
                }

                string imageUrl = model.ExistingImageUrl ?? string.Empty;

                // Yeni bir görsel yüklendiyse
                if (model.NewImage != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "announcements");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Eski görsel varsa sunucudan fiziksel olarak silebiliriz
                    if (!string.IsNullOrEmpty(announcement.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, announcement.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.NewImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.NewImage.CopyToAsync(fileStream);
                    }

                    imageUrl = "/images/announcements/" + uniqueFileName;
                }

                announcement.Title = model.Title;
                announcement.Summary = model.Summary;
                announcement.Content = model.Content;
                announcement.ImageUrl = imageUrl;
                announcement.PublishDate = model.PublishDate;
                announcement.Slug = model.Title.ToLower()
                    .Replace(" ", "-")
                    .Replace("ç", "c")
                    .Replace("ğ", "g")
                    .Replace("ı", "i")
                    .Replace("ö", "o")
                    .Replace("ş", "s")
                    .Replace("ü", "u");

                _announcementRepository.Update(announcement);
                await _announcementRepository.SaveChangesAsync();

                TempData["SuccessMessage"] = "Duyuru başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Lütfen alanları eksiksiz doldurun.";
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var announcement = await _announcementRepository.GetByIdAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            _announcementRepository.Delete(announcement);
            await _announcementRepository.SaveChangesAsync();
            TempData["Success"] = "Duyuru başarıyla silindi.";
            return RedirectToAction(nameof(Index));

        }
    }
}