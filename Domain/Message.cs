namespace Domain
{
    public class Message
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public User Autor { get; set; }
        public string MessageText { get; set; }
        public Chat Chats {  get; set; }
        public bool IsSoftDelete { get; set; }
    }
}
