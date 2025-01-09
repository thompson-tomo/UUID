using Microsoft.EntityFrameworkCore;
using System;

namespace UUIDSerializationEntityTests
{
    public class TestDbContext : DbContext
    {
        private readonly Action<ModelBuilder>? _onModelCreating;

        public TestDbContext(DbContextOptions<TestDbContext> options, Action<ModelBuilder>? onModelCreating = null)
            : base(options)
        {
            _onModelCreating = onModelCreating;
        }

        public DbSet<TestEntity> TestEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TestEntity>(entity =>
            {
                entity.ToTable("TestEntities");
            });

            _onModelCreating?.Invoke(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            // Default to binary storage
            configurationBuilder.UseUUIDAsBinary();
        }
    }
}