namespace MefaOzelEgitim.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; } = string.Empty;
        public string AgeRange { get; set; } = string.Empty;
        public string? Message { get; set; }
        public string Status { get; set; } = "Beklemede";
    }
}