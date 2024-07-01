namespace ChatRoom.Api.Dtos;

public record class RoomDto (int RoomId, string? RoomName, string? RoomDescription, int[]? RoomMembers, int RoomAdmin);

public record class CreateRoomDto (string? RoomName, string? RoomDescription, int[]? RoomMembers, int RoomAdmin);

public record class PreviewRoomDto (string? RoomDescription, int[]? RoomMembers, int RoomAdmin);

public record class AddMemberDto (int RoomId, int[]? RoomMembers);

public record class RemoveMemberDto (int RoomId, int[]? RoomMembers);