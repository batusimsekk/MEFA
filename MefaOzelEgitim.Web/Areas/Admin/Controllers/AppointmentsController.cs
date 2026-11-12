using MefaOzelEgitim.Domain.Entities;
using MefaOzelEgitim.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MefaOzelEgitim.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AppointmentsController : Controller
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        public AppointmentsController(IRepository<Appointment> appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        public async Task<IActionResult> Index()
        {
            var appointments = (await _appointmentRepository.GetAllAsync()).OrderByDescending(x => x.CreatedDate).ToList();
            return View(appointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _appointmentRepository.Delete(appointment);
            await _appointmentRepository.SaveChangesAsync();

            TempData["SuccessMessage"] = "Randevu kaydı silindi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = "İptal edildi.";
            _appointmentRepository.Update(appointment);
            await _appointmentRepository.SaveChangesAsync();

            TempData["InfoMessage"] = "Randevu iptal edildi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = "Onaylandı";
            _appointmentRepository.Update(appointment);
            await _appointmentRepository.SaveChangesAsync();

            TempData["SuccessMessage"] = "Randevu başarıyla onaylandı.";
            return RedirectToAction(nameof(Index));
        }


    }
}