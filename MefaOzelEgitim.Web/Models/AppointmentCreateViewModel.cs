using System.ComponentModel.DataAnnotations;

namespace MefaOzelEgitim.Web.Models
{
    public class AppointmentCreateViewModel
    {
        [Required(ErrorMessage = "Ad soyad alanı zorunludur.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon alanı zorunludur.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lütfen bir email giriniz.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lütfen bir eğitim/terapi türü seçiniz.")]
        public string ServiceType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lütfen bir yaş aralığı seçiniz.")]
        public string AgeRange { get; set; } = string.Empty;

        [Required(ErrorMessage = "Randevu tarihi seçilmelidir.")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Randevu saati seçilmelidir.")]
        public string AppointmentTime { get; set; } = string.Empty;

        public string? Message { get; set; }
    }
}