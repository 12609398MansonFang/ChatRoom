namespace ChatRoom.Api.Dtos;

public record class RoomDto (int RoomId, string? RoomName, string? RoomDescription, int[]? RoomMembers);

public record class CreateRoomDto (string? RoomName, string? RoomDescription, int[]? RoomMembers);

public record class JoinRoomDto (int RoomId, int[]? RoomMembers);
