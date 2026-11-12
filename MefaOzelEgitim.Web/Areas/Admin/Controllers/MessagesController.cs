
using MefaOzelEgitim.Domain.Entities;
using MefaOzelEgitim.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MefaOzelEgitim.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MessagesController : Controller
    {
        private readonly IRepository<ContactMessage> _contactRepository;
        public MessagesController(IRepository<ContactMessage> contactRepository)
        {
            _contactRepository = contactRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var messages = await _contactRepository.GetAllAsync();
            var orderedMessages = messages.OrderByDescending(m => m.CreatedDate).ToList();
            return View(orderedMessages);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var message = await _contactRepository.GetByIdAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            if (!message.IsRead)
            {
                message.IsRead = true;
                _contactRepository.Update(message);
                await _contactRepository.SaveChangesAsync();
            }

            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _contactRepository.GetByIdAsync(id);
            if (message != null)
            {
                _contactRepository.Delete(message);
                await _contactRepository.SaveChangesAsync();
                TempData["SuccessMessage"] = "Mesaj başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Silinecek mesaj bulunamadı.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var message = await _contactRepository.GetByIdAsync(id);
            if (message != null)
            {
                message.IsRead = true;
                _contactRepository.Update(message);
                await _contactRepository.SaveChangesAsync();
                TempData["SuccessMessage"] = "Mesaj okundu olarak işaretlendi.";
            }
            return RedirectToAction(nameof(Index));
        }


    }
}