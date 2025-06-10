using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.DbContext
{
    public interface IChatDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
