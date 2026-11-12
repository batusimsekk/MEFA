namespace MefaOzelEgitim.Domain.Entities
{
    public class GalleryImage : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}