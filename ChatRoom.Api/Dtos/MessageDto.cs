namespace ChatRoom.Api.Dtos;

public record class MessageDto(int MessageId, int MessageUserId, int MessageRoomId, string? MessageText);

public record class CreateMessageDto(int MessageUserId, int MessageRoomId, string? MessageText);