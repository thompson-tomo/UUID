namespace UUIDSerializationEntityDemo
{
    public class User
    {
        public int Id { get; set; }
        public UUID UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}