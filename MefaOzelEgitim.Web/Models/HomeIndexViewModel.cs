using MefaOzelEgitim.Domain.Entities;

namespace MefaOzelEgitim.Web.Models
{
    public class HomeIndexViewModel
    {
        // * KADRO
        public IEnumerable<Staff> StaffList { get; set; } = new List<Staff>();
        // * Hizmetler
        public IEnumerable<Service> ServiceList { get; set; } = new List<Service>();
        // * Duyurular
        public IEnumerable<Announcement> AnnouncementList { get; set; } = new List<Announcement>();
    }
}