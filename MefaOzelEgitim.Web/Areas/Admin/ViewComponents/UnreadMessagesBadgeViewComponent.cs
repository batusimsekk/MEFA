using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MefaOzelEgitim.Domain.Entities;
using MefaOzelEgitim.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MefaOzelEgitim.Web.Areas.Admin.ViewComponents
{
    public class UnreadMessagesBadgeViewComponent : ViewComponent
    {
        private readonly IRepository<ContactMessage> _contactRepository;
        public UnreadMessagesBadgeViewComponent(IRepository<ContactMessage> contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var messages = await _contactRepository.GetAllAsync();
            int unreadCount = messages.Count(m => !m.IsRead);

            return View(unreadCount);
        }
    }
}