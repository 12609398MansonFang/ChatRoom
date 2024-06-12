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
                };

                dbContext.Users.Add(user);
                dbContext.SaveChanges();

                var userDto = user.ToDto();

                return Results.Created($"/users/{user.UserId}", userDto);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use any logging framework, here we use Console for simplicity)
                Console.WriteLine($"Error: {ex.Message}");
                return Results.Problem("An error occurred while processing your request.");
            }
        });
        
        
        return group;
    }
}