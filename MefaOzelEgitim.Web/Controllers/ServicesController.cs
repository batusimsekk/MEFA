using MefaOzelEgitim.Domain.Entities;
using MefaOzelEgitim.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace MefaOzelEgitim.Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IRepository<Service> _serviceRepository;
        public ServicesController(IRepository<Service> serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<IActionResult> DetailAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }


    }
}