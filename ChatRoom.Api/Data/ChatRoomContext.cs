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
                new Message { MessageId = 1, MessageUserId = 1, MessageText = "Hello" }
            );
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, UserName = "Admin", UserEmail = "admin@test.com", UserPassword = "admin" },
                new User { UserId = 2, UserName = "BackEnd", UserEmail = "backend@test.com", UserPassword = "backend" },
                new User { UserId = 3, UserName = "FrontEnd", UserEmail = "frontend@test.com", UserPassword = "frontend" }
            );
            modelBuilder.Entity<Room>().HasData(
                new Room { RoomId = 1, RoomName = "Room 1", RoomDescription = "Admin Only", RoomMembers = [1]},
                new Room { RoomId = 2, RoomName = "Room 2", RoomDescription = "Developer Only", RoomMembers = [2,3]},
                new Room { RoomId = 3, RoomName = "Room 3", RoomDescription = "Everyone", RoomMembers = [1,2,3]}
            );
        }
    
}