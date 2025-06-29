﻿using NodaTime;

namespace Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public LocalDateTime CreatedDate { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MidleName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Chat> Chats { get; set; } = new List<Chat>();
        public bool IsSoftDelete { get; set; } = false;
    }
}
