using NodaTime;

namespace Domain
{
    public class Message
    {
        public Guid Id { get; set; }
        public LocalDateTime CreatedDate { get; set; }
        public User Autor { get; set; }
        public string MessageText { get; set; }
        public Chat Chats {  get; set; }
        public bool IsSoftDelete { get; set; }
    }
}
