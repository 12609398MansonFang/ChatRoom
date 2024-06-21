using ChatRoom.Api.Data;
using ChatRoom.Api.Dtos;
using ChatRoom.Api.Entities;
using ChatRoom.Api.Mapping;

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

        return group;
    }
}