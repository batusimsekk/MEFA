using System.ComponentModel.DataAnnotations;

namespace MefaOzelEgitim.Web.Areas.Admin.Models
{
    public class AnnouncementEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Duyuru başlığı zorunludur.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Özet alanını doldurunuz.")]
        public string Summary { get; set; } = string.Empty;

        [Required(ErrorMessage = "Duyuru içeriği zorunludur.")]
        public string Content { get; set; } = string.Empty;

        public string? ExistingImageUrl { get; set; } 

        public IFormFile? NewImage { get; set; } 

        public DateTime PublishDate { get; set; }
    }
}
