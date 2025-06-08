using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ChatAppDbContext : DbContext
    {
        public ChatAppDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();

            modelBuilder.Entity<Chat>();
            
            modelBuilder.Entity<Message>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
