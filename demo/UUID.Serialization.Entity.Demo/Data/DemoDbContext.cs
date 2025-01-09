using Microsoft.EntityFrameworkCore;

namespace UUIDSerializationEntityDemo
{
    public class DemoDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite("Data Source=demo.db")
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Configure all UUID properties to use binary storage by default
            configurationBuilder.UseUUIDAsBinary();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Example of per-property configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Override the default binary storage with string storage for this property
                entity.Property(e => e.UserId)
                    .HasConversion<UUIDToStringConverter>()
                    .IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedAt)
                    .IsRequired();
            });
        }
    }
}