namespace ChatRoom.Api.Dtos;

public record class MessageDto(int MessageId, string? MessageText);

public record class CreateMessageDto(string? MessageText);