using Microsoft.AspNetCore.Mvc;
using MefaOzelEgitim.Web.Models;
using MefaOzelEgitim.Domain.Interfaces;
using MefaOzelEgitim.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MefaOzelEgitim.Web.Controllers;

public class HomeController : Controller
{
    private readonly IRepository<Staff> _staffRepository;
    private readonly IRepository<Service> _serviceRepository;
    private readonly IRepository<Announcement> _announcementRepository;
    private readonly IRepository<ContactMessage> _contactRepository;
    public HomeController(IRepository<Staff> staffRepository, IRepository<Service> serviceRepository, IRepository<Announcement> announcementRepository,IRepository<ContactMessage> contactRepository)
    {
        _staffRepository = staffRepository;
        _serviceRepository = serviceRepository;
        _announcementRepository = announcementRepository;
        _contactRepository = contactRepository;
    }
    public async Task<IActionResult> Index()
    {
        var model = new HomeIndexViewModel
        {
            StaffList = await _staffRepository.GetAllAsync(),
            ServiceList = await _serviceRepository.GetAllAsync(),
            AnnouncementList = await _announcementRepository.GetAllAsync()
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult About()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContactSend(ContactSendViewModel model)
    {
        if (ModelState.IsValid)
        {
            var contactMessage = new ContactMessage
            {
                SenderName = model.SenderName,
                Email = model.Email,
                Phone = model.Phone,
                Subject = model.Subject,
                Message = model.Message,
                IsRead = false,
                CreatedDate = DateTime.UtcNow
            };

            await _contactRepository.AddAsync(contactMessage);
            await _contactRepository.SaveChangesAsync();

            TempData["SuccessMessage"] = "Mesajınız başarıyla gönderildi. En kısa sürede sizinle iletişime geçeceğiz.";
            return RedirectToAction(nameof(Contact));
        }

        return View(model);

    }



}
