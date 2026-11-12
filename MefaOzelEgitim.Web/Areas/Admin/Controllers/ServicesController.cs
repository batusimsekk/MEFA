using MefaOzelEgitim.Domain.Entities;
using MefaOzelEgitim.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MefaOzelEgitim.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ServicesController : Controller
    {

        private readonly IRepository<Service> _serviceRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ServicesController(IRepository<Service> serviceRepository, IWebHostEnvironment webHostEnvironment)
        {
            _serviceRepository = serviceRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var services = await _serviceRepository.GetAllAsync();
            return View(services);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service model, IFormFile? imageFile)
        {
            ModelState.Remove(nameof(model.ImageUrl));
            ModelState.Remove("imageFile");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "services");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                model.ImageUrl = "/images/services/" + uniqueFileName;
            }
            else
            {
                model.ImageUrl = string.Empty; // Görsel seçilmediyse boş bırakabiliriz
            }

            await _serviceRepository.AddAsync(model);
            await _serviceRepository.SaveChangesAsync();
            TempData["Success"] = "Yeni eğitim başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Service model, IFormFile imageFile)
        {
            // Görsel formdan string olarak gelmediği için ModelState hatasına takılmasın
            ModelState.Remove(nameof(model.ImageUrl));
            ModelState.Remove("imageFile");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingService = await _serviceRepository.GetByIdAsync(model.Id);
            if (existingService == null)
            {
                return NotFound();
            }

            // Yeni bir resim dosyası seçildiyse
            if (imageFile != null && imageFile.Length > 0)
            {
                // 1. Varsa eski resmi sunucudan (wwwroot'tan) tamamen silelim (çöplük olmasın)
                if (!string.IsNullOrEmpty(existingService.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingService.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // 2. Yeni resmi wwwroot/images/services klasörüne kaydedelim
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "services");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                // Yeni görselin yolunu entity'ye atıyoruz
                existingService.ImageUrl = "/images/services/" + uniqueFileName;
            }

            existingService.Title = model.Title;
            existingService.Slug = model.Slug;
            existingService.ShortDescription = model.ShortDescription;
            existingService.Content = model.Content;
            existingService.IconClass = model.IconClass;

            _serviceRepository.Update(existingService);
            await _serviceRepository.SaveChangesAsync();
            TempData["Success"] = "Eğitim bilgileri başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _serviceRepository.Delete(service);
            await _serviceRepository.SaveChangesAsync();
            TempData["Success"] = "Eğitim başarıyla silindi.";
            return RedirectToAction(nameof(Index));

        }


    }
}