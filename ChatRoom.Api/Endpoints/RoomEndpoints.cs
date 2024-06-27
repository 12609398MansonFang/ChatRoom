using ChatRoom.Api.Data;
using ChatRoom.Api.Dtos;
using ChatRoom.Api.Entities;
using ChatRoom.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.Api.Endpoints;

public static class RoomEndpoints
{
    public static RouteGroupBuilder MapRoomEndpoints(this WebApplication app){
        var group = app.MapGroup("rooms");

        // API to Get All Rooms In the Data Base
        group.MapGet("/all", (ChatRoomContext dbContext) => 
        {
            try {
                var rooms = dbContext.Rooms.ToList();
                var roomDto = rooms.Select(room => room.ToDto());
                return Results.Ok(roomDto);
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
                return Results.Problem("An error occurred while processing your request to get all rooms.");
            }
        });

        // API to Get The Rooms Specific to a Member 
        group.MapGet("/user{memberId}", (int memberId, ChatRoomContext dbContext) => 
        {
            try {
                var rooms = dbContext.Rooms.Where(r => r.RoomMembers != null && r.RoomMembers.Contains(memberId)).ToList();
                var roomDto = rooms.Select(room => room.ToDto());
                return Results.Ok(roomDto);
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
                return Results.Problem("An error occurred while processing your request to user specific rooms.");
            }
        });

        // API to Get a Preview for a Room
        group.MapGet("/preview{roomId}", (int roomId, ChatRoomContext dbContext) => 
        {
            try {
                var room = dbContext.Rooms.FirstOrDefault(r => r.RoomId == roomId);
                if (room == null)
                {
                    return Results.NotFound($"Room with ID {roomId} not found.");
                }
                var previewRoomDto = new PreviewRoomDto(room.RoomDescription, room.RoomMembers, room.RoomAdmin);
                return Results.Ok(previewRoomDto);
            }
            catch (Exception ex){
                Console.WriteLine($"Error: {ex.Message}");
                return Results.Problem("An error occurred while processing your request to preview a room.");
            }
        });

        // API to Create a Room
        group.MapPost("/create", (CreateRoomDto createRoomDto, ChatRoomContext dbContext) => 
        {
            try {
                if (createRoomDto == null || 
                    string.IsNullOrEmpty(createRoomDto.RoomName) ||
                    string.IsNullOrEmpty(createRoomDto.RoomDescription) || 
                    createRoomDto.RoomMembers == null ||
                    createRoomDto.RoomMembers.Length == 0 ||
                    createRoomDto.RoomAdmin == 0)
                {
                    return Results.BadRequest("Room name, description, and members are required.");
                } else {
                    var room = new Room {
                        RoomName = createRoomDto.RoomName,
                        RoomDescription = createRoomDto.RoomDescription,
                        RoomMembers = createRoomDto.RoomMembers,
                        RoomAdmin = createRoomDto.RoomAdmin
                    };

                    dbContext.Rooms.Add(room);
                    dbContext.SaveChanges();

                    var roomDto = room.ToDto();
                    
                    return Results.Ok(roomDto);

                    }
            }
            catch (Exception ex){
                Console.WriteLine($"Error: {ex.Message}");
                return Results.Problem("An error occurred while processing your request to create rooms.");
            }
        });

        //API to add User to a Room
        group.MapPut("/add/room{roomId}/user{userId}", (int roomId, int userId, ChatRoomContext dbContext) => 
        {
            try {
                var room = dbContext.Rooms.FirstOrDefault(r => r.RoomId == roomId);
                if (room == null)
                {
                    return Results.NotFound($"Room with ID {roomId} not found.");
                }

                if (room.RoomMembers == null)
                {
                    room.RoomMembers = new int[] { userId };
                }
                else if (!room.RoomMembers.Contains(userId))
                {
                    room.RoomMembers = room.RoomMembers.Append(userId).ToArray();
                }
                else
                {
                    return Results.BadRequest("User is already a member of the room.");
                }

                dbContext.SaveChanges();
                
                var roomDto = room.ToDto();
                return Results.Ok(roomDto);
            }
            catch (Exception ex){
                Console.WriteLine($"Error: {ex.Message}");
                return Results.Problem("An error occurred while processing your request to add a user to the room.");
            }
        });

        //API to Delete Rooms
        group.MapDelete("/delete{roomId}", (int roomId, ChatRoomContext dbContext) => 
        {
            try {
                var room = dbContext.Rooms.FirstOrDefault(r => r.RoomId == roomId);
                if (room == null)
                {
                    return Results.NotFound($"User doesn't exist");
                }

                dbContext.Rooms.Remove(room);
                dbContext.SaveChanges();
                return Results.NoContent();
            }
            catch (Exception ex){
                Console.WriteLine($"Error: {ex.Message}");
                return Results.Problem($"An error occurred while processing your request to delete room {roomId}.");
            }
        });

        return group;
    }
}