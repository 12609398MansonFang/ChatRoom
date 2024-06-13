namespace ChatRoom.Api.Dtos;

public record class MessageDto(int MessageId, int MessageUserId, string? MessageText);

public record class CreateMessageDto(int MessageUserId, string? MessageText);