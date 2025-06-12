using Application.DbContext;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ChatAppDbContext : DbContext, IChatDbContext
    {
        public ChatAppDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Chats);

            modelBuilder.Entity<Chat>()
                .HasMany(c => c.Users);
            
            modelBuilder.Entity<Message>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
