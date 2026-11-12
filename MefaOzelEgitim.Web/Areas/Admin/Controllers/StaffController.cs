using MefaOzelEgitim.Domain.Entities;
using MefaOzelEgitim.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MefaOzelEgitim.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StaffController : Controller
    {
        private readonly IRepository<Staff> _staffRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public StaffController(IRepository<Staff> staffRepository, IWebHostEnvironment webHostEnvironment)
        {
            _staffRepository = staffRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var staffList = await _staffRepository.GetAllAsync();
            var orderedList = staffList.OrderBy(s => s.DisplayOrder).ToList();
            return View(orderedList);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            _staffRepository.Delete(staff);
            await _staffRepository.SaveChangesAsync();
            TempData["Success"] = "Personel başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Staff model, IFormFile? imageFile)
        {
            var existingStaff = await _staffRepository.GetByIdAsync(model.Id);
            if (existingStaff == null)
            {
                return NotFound();
            }

            existingStaff.FullName = model.FullName;
            existingStaff.Title = model.Title;
            existingStaff.Department = model.Department;       
            existingStaff.Biography = model.Biography;
            existingStaff.Education = model.Education;         
            existingStaff.Certificates = model.Certificates;
            existingStaff.Specialties = model.Specialties;
            existingStaff.DisplayOrder = model.DisplayOrder;

            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "staff");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var uploadPath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                existingStaff.ImageUrl = "/images/staff/" + fileName;
            }

            _staffRepository.Update(existingStaff);
            await _staffRepository.SaveChangesAsync();
            TempData["Success"] = "Personel bilgileri başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Staff model, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Güvenli ve dinamik dosya yükleme (IWebHostEnvironment ile)
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "staff");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var uploadPath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                model.ImageUrl = "/images/staff/" + fileName;
            }

            await _staffRepository.AddAsync(model);
            await _staffRepository.SaveChangesAsync();
            TempData["Success"] = "Yeni uzman başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }
    }
}