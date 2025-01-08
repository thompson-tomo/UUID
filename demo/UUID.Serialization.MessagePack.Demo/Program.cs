using MessagePack;
using MessagePack.Resolvers;

namespace UUIDSerializationMessagePackDemo
{
    [MessagePackObject]
    public class UserRecord
    {
        [Key(0)]
        public int Id { get; set; }

        [Key(1)]
        public UUID UserId { get; set; }

        [Key(2)]
        public string Name { get; set; } = string.Empty;
    }

    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("UUID MessagePack Serialization Demo");
            Console.WriteLine("---------------------------------");

            // Configure MessagePack with UUID formatter
            MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard
                .WithResolver(CompositeResolver.Create(
                    new[] { new UUIDFormatter() },
                    new[] { StandardResolver.Instance }));

            // Create demo data
            UserRecord user = new()
            {
                Id = 1,
                UserId = UUID.New(),
                Name = "John Doe"
            };

            Console.WriteLine($"\nOriginal user:");
            Console.WriteLine($"Id: {user.Id}");
            Console.WriteLine($"UserId: {user.UserId}");
            Console.WriteLine($"Name: {user.Name}");

            // Serialize
            byte[] bytes = MessagePackSerializer.Serialize(user, options);
            Console.WriteLine($"\nSerialized size: {bytes.Length} bytes");

            // Deserialize
            UserRecord result = MessagePackSerializer.Deserialize<UserRecord>(bytes, options);

            Console.WriteLine("\nDeserialized user:");
            Console.WriteLine($"Id: {result.Id}");
            Console.WriteLine($"UserId: {result.UserId}");
            Console.WriteLine($"Name: {result.Name}");

            // Verify
            Console.WriteLine($"\nUUIDs match: {user.UserId == result.UserId}");

            // Collection example
            UserRecord[] users = new[]
            {
                new UserRecord { Id = 1, UserId = UUID.New(), Name = "User 1" },
                new UserRecord { Id = 2, UserId = UUID.New(), Name = "User 2" },
                new UserRecord { Id = 3, UserId = UUID.New(), Name = "User 3" }
            };

            // Serialize collection
            byte[] collectionBytes = MessagePackSerializer.Serialize(users, options);
            Console.WriteLine($"\nSerialized collection size: {collectionBytes.Length} bytes");

            // Deserialize collection
            UserRecord[] resultCollection = MessagePackSerializer.Deserialize<UserRecord[]>(collectionBytes, options);

            Console.WriteLine("\nDeserialized collection:");
            foreach (UserRecord item in resultCollection)
            {
                Console.WriteLine($"User {item.Id}: {item.UserId}");
            }
        }
    }
}