using Microsoft.EntityFrameworkCore;
using System;

namespace UUIDSerializationEntityTests
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        public DbSet<TestEntity> TestEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TestEntity>(entity =>
            {
                entity.ToTable("TestEntities");

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