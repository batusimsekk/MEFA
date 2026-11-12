using System.ComponentModel.DataAnnotations;

namespace MefaOzelEgitim.Web.Models
{
    public class ContactSendViewModel
    {
        [Required(ErrorMessage = "Ad Soyad alanı zorunludur.")]
        public string SenderName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mesaj alanı zorunludur.")]
        public string Message { get; set; } = string.Empty;
    }
}