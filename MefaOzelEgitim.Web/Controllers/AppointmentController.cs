using MefaOzelEgitim.DataAccess.Context;
using MefaOzelEgitim.Domain.Entities;
using MefaOzelEgitim.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MefaOzelEgitim.Web.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly MefaDbContext _context;
        public AppointmentController(MefaDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyAppointments()
        {
            var userEmail = User.Identity!.Name; 
            
            var appointments = await _context.Appointments
                .Where(x => x.Email == userEmail)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var services = await _context.Services.Where(x => x.IsActive).ToListAsync();
            ViewBag.ServiceList = new SelectList(services, "Title", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appointment = new Appointment
                {
                    FullName = model.FullName,
                    Phone = model.Phone,
                    Email = model.Email,  
                    ServiceType = model.ServiceType,
                    AgeRange = model.AgeRange,
                    AppointmentDate = model.AppointmentDate,
                    AppointmentTime = model.AppointmentTime,
                    Message = model.Message,
                    Status = "Beklemede",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Randevu talebiniz başarıyla oluşturuldu!";
                return RedirectToAction(nameof(Index));
            }

            var services = await _context.Services.Where(x => x.IsActive).ToListAsync();
            ViewBag.ServiceList = new SelectList(services, "Title", "Title");

            TempData["ErrorMessage"] = "Lütfen formdaki hataları giderin.";
            return View("Index", model);
        }
    }
}