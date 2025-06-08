namespace Domain
{
    public class Chat
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Message>? Messages { get; set; }
        public bool IsSoftDelete { get; set; }
    }
}
