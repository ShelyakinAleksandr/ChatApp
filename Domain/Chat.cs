using NodaTime;

namespace Domain
{
    public class Chat
    {
        public Guid Id { get; set; }
        public LocalDateTime CreateDate { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public bool IsSoftDelete { get; set; } = false;
    }
}
