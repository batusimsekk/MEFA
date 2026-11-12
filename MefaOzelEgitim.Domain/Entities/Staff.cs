namespace MefaOzelEgitim.Domain.Entities
{
    public class Staff : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public string Education { get; set; } = string.Empty;
        public string Certificates { get; set; } = string.Empty;
        public string Specialties { get; set; } = string.Empty;
    }
}