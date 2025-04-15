using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace UUIDSerializationDapperDemo
{
    public class UserRecord
    {
        public int Id { get; set; }
        public UUID UserId { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class Program
    {
        private const string DatabasePath = "demo.db";

        public static void Main()
        {
            // Register UUID type handlers
            SqlMapper.AddTypeHandler(new Base64UUIDHandler());
            SqlMapper.AddTypeHandler(new BinaryUUIDHandler());
            SqlMapper.AddTypeHandler(new StringUUIDHandler());

            Console.WriteLine("UUID Dapper Serialization Demo (SQLite)");
            Console.WriteLine("--------------------------------------");

            // Delete existing database file if exists
            if (File.Exists(DatabasePath))
            {
                File.Delete(DatabasePath);
            }

            using SqliteConnection connection = new($"Data Source={DatabasePath}");
            connection.Open();

            // Create demo table
            CreateDemoTable(connection);

            // Insert demo data
            UserRecord user = new()
            {
                Id = 1,
                UserId = UUID.New(),
                Name = "John Doe"
            };

            connection.Execute(@"
                INSERT INTO Users (Id, UserId, Name) 
                VALUES (@Id, @UserId, @Name)", user);

            Console.WriteLine($"\nInserted user with UUID: {user.UserId}");

            // Query demo data
            UserRecord result = connection.QuerySingle<UserRecord>(
                "SELECT * FROM Users WHERE Id = @Id",
                new { Id = 1 });

            Console.WriteLine("\nQueried user:");
            Console.WriteLine($"Id: {result.Id}");
            Console.WriteLine($"UserId: {result.UserId}");
            Console.WriteLine($"Name: {result.Name}");

            // Verify
            Console.WriteLine($"\nUUIDs match: {user.UserId == result.UserId}");

            // Collection example
            UserRecord[] users = new[]
            {
                new UserRecord { Id = 2, UserId = UUID.New(), Name = "User 2" },
                new UserRecord { Id = 3, UserId = UUID.New(), Name = "User 3" },
                new UserRecord { Id = 4, UserId = UUID.New(), Name = "User 4" }
            };

            // Insert collection
            connection.Execute(@"
                INSERT INTO Users (Id, UserId, Name) 
                VALUES (@Id, @UserId, @Name)", users);

            // Query collection
            IEnumerable<UserRecord> results = connection.Query<UserRecord>("SELECT * FROM Users ORDER BY Id");

            Console.WriteLine("\nAll users:");
            foreach (UserRecord item in results)
            {
                Console.WriteLine($"User {item.Id}: {item.UserId}");
            }

            // Close connection
            connection.Close();

            Console.WriteLine($"\nDemo database created at: {Path.GetFullPath(DatabasePath)}");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void CreateDemoTable(IDbConnection connection)
        {
            connection.Execute(@"
                DROP TABLE IF EXISTS Users;
                CREATE TABLE Users (
                    Id INTEGER PRIMARY KEY,
                    UserId BLOB,  -- For binary storage
                    Name TEXT
                );");
        }
    }
}