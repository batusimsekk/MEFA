using MefaOzelEgitim.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MefaOzelEgitim.DataAccess.Context
{
    public class MefaDbContext : IdentityDbContext
    {
        public MefaDbContext(DbContextOptions<MefaDbContext> options) : base(options)
        {

        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<GalleryImage> GalleryImages { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ! Fluent API kısıtlamaları ve tablo kuralları
            modelBuilder.Entity<Service>().Property(s => s.Title).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<Service>().Property(s => s.Slug).IsRequired().HasMaxLength(160);
            modelBuilder.Entity<Staff>().Property(s => s.FullName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Announcement>().Property(s => s.Title).IsRequired().HasMaxLength(200);

        }
    }
}