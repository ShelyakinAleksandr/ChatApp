namespace Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MidleName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Chat>? Chats { get; set; }
        public bool IsSoftDelete { get; set; }
    }
}
