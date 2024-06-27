namespace ChatRoom.Api.Dtos;

public record class UserDto(int UserId, string? UserName, string? UserEmail, string? UserPassword);

public record class CreateUserDto(string? UserName, string? UserEmail, string? UserPassword);

public record class LogIntoUserDto(int UserId);