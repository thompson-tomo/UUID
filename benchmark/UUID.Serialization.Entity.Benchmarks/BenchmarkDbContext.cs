using Microsoft.EntityFrameworkCore;

namespace UUIDSerializationEntityBenchmarks
{
    public class BenchmarkDbContext : DbContext
    {
        public BenchmarkDbContext(DbContextOptions<BenchmarkDbContext> options)
            : base(options)
        {
        }

        public DbSet<BenchmarkEntity> Entities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BenchmarkEntity>(entity =>
            {
                entity.ToTable("BenchmarkEntities");

                entity.Property(e => e.ByteUUID)
                    .HasConversion<UUIDToBytesConverter>();

                entity.Property(e => e.StringUUID)
                    .HasConversion<UUIDToStringConverter>();

                entity.Property(e => e.Base64UUID)
                    .HasConversion<UUIDToBase64Converter>();
            });
        }
    }
}