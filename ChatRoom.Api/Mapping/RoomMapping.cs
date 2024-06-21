using ChatRoom.Api.Dtos;
using ChatRoom.Api.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace ChatRoom.Api.Mapping;

public static class RoomMapping 
{
    public static RoomDto ToDto(this Room room)
    {
        return new RoomDto(room.RoomId, room.RoomName, room.RoomDescription, room.RoomMembers);
    }
}