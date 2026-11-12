using MefaOzelEgitim.Domain.Entities;
using MefaOzelEgitim.Domain.Interfaces;
using MefaOzelEgitim.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace MefaOzelEgitim.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IRepository<Service> _serviceRepository;
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<Staff> _staffRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(IRepository<Service> serviceRepository, IRepository<Appointment> appointmentRepository, IRepository<Staff> staffRepository, UserManager<IdentityUser> userManager)
        {
            _serviceRepository = serviceRepository;
            _appointmentRepository = appointmentRepository;
            _staffRepository = staffRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _serviceRepository.GetAllAsync();
            var appointments = await _appointmentRepository.GetAllAsync();
            var staffList = await _staffRepository.GetAllAsync();

            var model = new DashboardViewModel
            {
                ServiceCount = services.Count(),
                AppointmentCount = appointments.Count(),
                StaffCount = staffList.Count(),
                UserCount = _userManager.Users.Count()
            };
            return View(model);
        }

    }
}