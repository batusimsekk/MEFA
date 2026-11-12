namespace MefaOzelEgitim.Domain.Entities
{
    public class Service : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string IconClass { get; set; } = string.Empty;
    }
}