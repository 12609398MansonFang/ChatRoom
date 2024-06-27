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

        group.MapGet("/", (ChatRoomContext dbContext) => 
        {
            var rooms = dbContext.Rooms.ToList();
            var roomDtos = rooms.Select(room => room.ToDto());
            return Results.Ok(roomDtos);

        });

        group.MapGet("/{memberId}", (int memberId, ChatRoomContext dbContext) => {
            var rooms = dbContext.Rooms
                .Where(r => r.RoomMembers != null && r.RoomMembers.Contains(memberId))
                .ToList();
            var roomDtos = rooms.Select(room => room.ToDto());
            return Results.Ok(roomDtos);

        });

        group.MapPost("/create", (CreateRoomDto createRoomDto, ChatRoomContext dbContext) => 
        {
            if (createRoomDto == null || 
                string.IsNullOrEmpty(createRoomDto.RoomName) ||
                string.IsNullOrEmpty(createRoomDto.RoomDescription) || 
                createRoomDto.RoomMembers == null ||
                createRoomDto.RoomMembers.Length == 0)
            {
                return Results.BadRequest("Room name, description, and members are required.");
            }
            
            var room = new Room {
                RoomName = createRoomDto.RoomName,
                RoomDescription = createRoomDto.RoomDescription,
                RoomMembers = createRoomDto.RoomMembers
            };

            dbContext.Rooms.Add(room);
            dbContext.SaveChanges();

            var roomDto = room.ToDto();
            
            return Results.Ok(roomDto);
        });

        return group;
    }
}