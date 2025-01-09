# UUID.Serialization.Entity

Entity Framework Core value converters for UUID types. This package provides converters for storing UUID values in different formats:

- Binary (16 bytes)
- Base64 (24 characters)
- String (32 characters)

## Installation

```bash
dotnet add package UUID.Serialization.Entity
```

## Usage

### Global Configuration

Configure all UUID properties in your DbContext to use a specific storage format:

```csharp
public class MyDbContext : DbContext
{
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Use binary storage (most efficient, 16 bytes)
        configurationBuilder.UseUUIDAsBinary();

        // Or use string storage (32 characters)
        // configurationBuilder.UseUUIDAsString();

        // Or use base64 storage (24 characters)
        // configurationBuilder.UseUUIDAsBase64();
    }
}
```

### Per-Property Configuration

Configure individual properties to use specific storage formats:

```csharp
public class MyDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(e => e.Id)
            .HasConversion<UUIDToBytesConverter>();

        modelBuilder.Entity<AuditLog>()
            .Property(e => e.Id)
            .HasConversion<UUIDToStringConverter>();

        modelBuilder.Entity<Session>()
            .Property(e => e.Id)
            .HasConversion<UUIDToBase64Converter>();
    }
}
```

### Database Schema Examples

```sql
-- For binary storage (most efficient, 16 bytes)
CREATE TABLE Users (
    Id INTEGER PRIMARY KEY,
    UserId BLOB  -- SQLite
    -- or: UserId BINARY(16)  -- SQL Server
    -- or: UserId BYTEA       -- PostgreSQL
);

-- For string storage (32 characters)
CREATE TABLE AuditLogs (
    Id INTEGER PRIMARY KEY,
    UserId CHAR(32)  -- Fixed-length string
);

-- For base64 storage (24 characters)
CREATE TABLE Sessions (
    Id INTEGER PRIMARY KEY,
    UserId VARCHAR(24)  -- Base64 encoded
);
```

## Features

- **Multiple Storage Formats**
  - Binary storage (16 bytes) for maximum efficiency
  - Base64 storage (24 characters) for balanced approach
  - String storage (32 characters) for human readability

- **Flexible Configuration**
  - Global configuration through `ConfigureConventions`
  - Per-property configuration using fluent API
  - Easy to switch between storage formats

- **Database Compatibility**
  - Works with all major databases (SQL Server, PostgreSQL, SQLite, etc.)
  - Automatic type mapping
  - Proper size hints for database schema generation

- **Performance Optimized**
  - Zero-allocation conversions where possible
  - Efficient byte array handling
  - Minimal memory footprint

## Best Practices

1. Use base64 storage (`UseUUIDAsBase64`) for a balance between readability and storage size.
2. Use binary storage (`UseUUIDAsBinary`) for the most efficient storage and best performance.
3. Use string storage (`UseUUIDAsString`) when you need human-readable values or easier debugging.

## Requirements

- UUID library
- Supports .NET 6.0 or later
- Microsoft.EntityFrameworkCore 6.0.0 or later

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the terms of the LICENSE file included in the root directory.