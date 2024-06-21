using ChatRoom.Api.Data;
using ChatRoom.Api.Dtos;
using ChatRoom.Api.Entities;
using ChatRoom.Api.Mapping;

namespace ChatRoom.Api.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app){
        var group = app.MapGroup("users");

        group.MapGet("/", (ChatRoomContext dbContext) => 
        {
            var users = dbContext.Users.ToList();
            var userDtos = users.Select(users => users.ToDto());
            return Results.Ok(userDtos);
        });

        group.MapPost("/", (CreateUserDto createUserDto, ChatRoomContext dbContext) => 
        {
            try
            {
                if (createUserDto == null || string.IsNullOrEmpty(createUserDto.UserName))
                {
                    return Results.BadRequest("Invalid User data.");
                }

                var user = new User
                {
                    UserName = createUserDto.UserName,
                    UserEmail = createUserDto.UserEmail,
                    UserPassword = createUserDto.UserPassword
                };

                dbContext.Users.Add(user);
                dbContext.SaveChanges();

                var userDto = user.ToDto();

                return Results.Created($"/users/{user.UserId}", userDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Results.Problem("An error occurred while processing your request.");
            }
        });

        group.MapPost("/login", (LogIntoUserDto logIntoUserDto, ChatRoomContext dbContext) => 
        {
            try
            {
                var user = dbContext.Users.SingleOrDefault(u => u.UserEmail == logIntoUserDto.UserEmail);
                if (user == null)
                {
                    Console.WriteLine("User not found.");
                    return Results.NotFound();
                } else if (logIntoUserDto.UserPassword == user.UserPassword){
                    Console.WriteLine("Password match. Fetching user data.");
                    var userDto = user.ToDto();
                    return Results.Ok(userDto);
                } else {
                    Console.WriteLine("Invalid password.");
                    return Results.Unauthorized();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Results.Problem("An error occurred while processing your request.");
            }
        });

        return group;
    }
}