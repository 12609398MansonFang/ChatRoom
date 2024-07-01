using ChatRoom.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.Api.Data;

public class ChatRoomContext(DbContextOptions<ChatRoomContext> options)
                    :DbContext(options)
{
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Room> Rooms => Set<Room>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().HasData(
                new Message { MessageId = 1, MessageRoomId = 1, MessageUserId = 1, MessageText = "This message is from Admins" },
                new Message { MessageId = 2, MessageRoomId = 2, MessageUserId = 2, MessageText = "This message is from BackEnd" },
                new Message { MessageId = 3, MessageRoomId = 3, MessageUserId = 3, MessageText = "This message is from FrontEnd" }
            );
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, UserName = "Admin", UserEmail = "admin@test.com", UserPassword = "admin" },
                new User { UserId = 2, UserName = "BackEnd", UserEmail = "backend@test.com", UserPassword = "backend" },
                new User { UserId = 3, UserName = "FrontEnd", UserEmail = "frontend@test.com", UserPassword = "frontend" }
            );
            modelBuilder.Entity<Room>().HasData(
                new Room { RoomId = 1, RoomName = "Admin Room", RoomDescription = "Admin is Admin", RoomMembers = [1], RoomAdmin = 1},
                new Room { RoomId = 2, RoomName = "BackEnd Room", RoomDescription = "BackEnd is Admin", RoomMembers = [2], RoomAdmin = 2},
                new Room { RoomId = 3, RoomName = "FrontEnd Room", RoomDescription = "FrontEnd is Admin", RoomMembers = [3], RoomAdmin = 3}
            );
        }
    
}