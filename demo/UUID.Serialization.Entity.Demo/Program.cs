using Microsoft.EntityFrameworkCore;

namespace UUIDSerializationEntityDemo
{
    public class Program
    {
        public static async Task Main()
        {
            // Create and initialize the database
            using DemoDbContext db = new();

            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();

            Console.WriteLine("UUID Entity Framework Core Demo");
            Console.WriteLine("==============================\n");

            // Create users with different UUID formats
            User[] users = new[]
            {
                new User
                {
                    UserId = UUID.New(),
                    Name = "John Doe",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new User
                {
                    UserId = UUID.New(),
                    Name = "Jane Smith",
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new User
                {
                    UserId = UUID.New(),
                    Name = "Alice Johnson",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new User
                {
                    UserId = UUID.New(),
                    Name = "Bob Wilson",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            // Batch insert users
            Console.WriteLine("Inserting users...");
            db.Users.AddRange(users);
            await db.SaveChangesAsync();
            Console.WriteLine("Users inserted successfully.\n");

            // Basic query examples
            Console.WriteLine("Basic Query Examples:");
            Console.WriteLine("---------------------");

            try
            {
                // 1. Query all users
                Console.WriteLine("1. All Users:");
                List<User> allUsers = await db.Users.OrderBy(u => u.CreatedAt).ToListAsync();
                DisplayUsers(allUsers);

                // 2. Query by specific UUID
                Console.WriteLine("\n2. Query by UUID:");
                UUID targetUserId = users[1].UserId;
                User? userByUUID = await db.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == targetUserId);
                DisplayUser(userByUUID);

                // 3. Query users created after a specific date
                Console.WriteLine("\n3. Recent Users (last 2 days):");
                DateTime twoDaysAgo = DateTime.UtcNow.AddDays(-2);
                List<User> recentUsers = await db.Users
                    .Where(u => u.CreatedAt >= twoDaysAgo)
                    .OrderByDescending(u => u.CreatedAt)
                    .ToListAsync();
                DisplayUsers(recentUsers);

                // 4. Query with multiple conditions
                Console.WriteLine("\n4. Users with specific criteria:");
                List<User> filteredUsers = await db.Users
                    .Where(u => u.CreatedAt >= DateTime.UtcNow.AddDays(-3) && u.Name.StartsWith("J"))
                    .OrderBy(u => u.Name)
                    .ToListAsync();
                DisplayUsers(filteredUsers);

                // 5. Batch update example
                Console.WriteLine("\n5. Batch Update Example:");
                int updatedCount = await db.Users
                    .Where(u => u.CreatedAt <= DateTime.UtcNow.AddDays(-4))
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(u => u.Name, u => u.Name + " (Veteran)"));
                Console.WriteLine($"Updated {updatedCount} veteran users\n");

                // Display updated users
                allUsers = await db.Users.OrderBy(u => u.CreatedAt).ToListAsync();
                DisplayUsers(allUsers);

                // 6. UUID Collection Query
                Console.WriteLine("\n6. Query by UUID Collection:");
                UUID targetUUID1 = users[0].UserId;
                UUID targetUUID2 = users[2].UserId;
                List<User> usersByUUIDs = await db.Users
                    .AsNoTracking()
                    .Where(u => u.UserId == targetUUID1 || u.UserId == targetUUID2)
                    .OrderBy(u => u.Name)
                    .ToListAsync();
                DisplayUsers(usersByUUIDs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void DisplayUsers(IEnumerable<User> users)
        {
            foreach (User user in users)
            {
                DisplayUser(user);
            }
        }

        private static void DisplayUser(User? user)
        {
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            Console.WriteLine($"ID: {user.Id}");
            Console.WriteLine($"UUID: {user.UserId}");
            Console.WriteLine($"Name: {user.Name}");
            Console.WriteLine($"Created: {user.CreatedAt:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine();
        }
    }
}