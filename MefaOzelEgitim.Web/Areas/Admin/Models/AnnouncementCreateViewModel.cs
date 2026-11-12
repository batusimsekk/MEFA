using System.ComponentModel.DataAnnotations;

namespace MefaOzelEgitim.Web.Areas.Admin.Models
{
    public class AnnouncementCreateViewModel
    {
        [Required(ErrorMessage = "Duyuru başlığı zorunludur.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Özet alanını doldurunuz.")]
        public string Summary { get; set; } = string.Empty;

        [Required(ErrorMessage = "Duyuru içeriği zorunludur.")]
        public string Content { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }

        public DateTime PublishDate { get; set; } = DateTime.Now;
    }
}