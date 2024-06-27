namespace ChatRoom.Api.Dtos;

public record class RoomDto (int RoomId, string? RoomName, string? RoomDescription, int[]? RoomMembers, int RoomAdmin);

public record class CreateRoomDto (string? RoomName, string? RoomDescription, int[]? RoomMembers, int RoomAdmin);

public record class PreviewRoomDto (string? RoomDescription, int[]? RoomMembers, int RoomAdmin);

