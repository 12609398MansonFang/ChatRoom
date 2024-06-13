using ChatRoom.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.Api.Data;

public class ChatRoomContext(DbContextOptions<ChatRoomContext> options)
                    :DbContext(options)
{
    public DbSet<Message> Messages => Set<Message>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<Message>().HasData(
            new { MessageId = 1, MessageUserId = 0, MessageText = "Hello" }
        );
    }
    
}