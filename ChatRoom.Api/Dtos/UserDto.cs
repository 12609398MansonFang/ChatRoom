namespace ChatRoom.Api.Dtos;

public record class UserDto(int UserId, string? UserName, string? UserEmail);

public record class CreateUserDto(string? UserName, string? UserEmail);