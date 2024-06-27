using Microsoft.AspNetCore.SignalR;

namespace ChatRoom.Api.Entities;

public class Room
{
    public int RoomId {get; set;}
    public string? RoomName {get; set;}
    public string? RoomDescription {get; set;}
    public int[]? RoomMembers {get; set;}
    public int RoomAdmin { get; set; }
}