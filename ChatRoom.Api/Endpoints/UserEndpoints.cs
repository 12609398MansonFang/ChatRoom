using ChatRoom.Api.Data;
using ChatRoom.Api.Dtos;
using ChatRoom.Api.Entities;
using ChatRoom.Api.Mapping;

namespace ChatRoom.Api.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app){
        var group = app.MapGroup("users");
        
        // API to get All Users In the Database
        group.MapGet("/all", (ChatRoomContext dbContext) => 
        {
            try {
                var users = dbContext.Users.ToList();
                var userDto = users.Select(users => users.ToDto());
                return Results.Ok(userDto);
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
                return Results.Problem("An error occurred while processing your request to get all users.");
            }            
        });
        
        // API to Create a User
        group.MapPost("/create", (CreateUserDto createUserDto, ChatRoomContext dbContext) => 
        {
            try
            {
                if (createUserDto == null || string.IsNullOrEmpty(createUserDto.UserName) || string.IsNullOrEmpty(createUserDto.UserEmail))
                {
                    return Results.BadRequest("Invalid User data.");
                }

                var existingUser = dbContext.Users.FirstOrDefault(u => u.UserEmail == createUserDto.UserEmail);
                if (existingUser != null)
                {
                    return Results.Conflict("A user with the same email already exists.");
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
                return Results.Problem("An error occurred while processing your request to create a user.");
            }
        });

        // API to Login by Entering Email and Password
        group.MapGet("/login/email/password/{email}/{password}", (string email, string password, ChatRoomContext dbContext) =>
        {
            try
            {
                var user = dbContext.Users.SingleOrDefault(u => u.UserEmail == email && u.UserPassword == password);
                if (user == null)
                {
                    return Results.NotFound($"User with email {email} not found or password incorrect.");
                }
                var logIntoUserDto = new LogIntoUserDto(user.UserId);
                return Results.Ok(logIntoUserDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Results.Problem("An error occurred while processing your request to login.");
            }
        });

        // API to Delete Account by Entering Email and Password
        group.MapDelete("/delete/email/password/{email}/{password}",(string email, string password, ChatRoomContext dbContext) => 
        {
            try {
                var user = dbContext.Users.SingleOrDefault(u => u.UserEmail == email && u.UserPassword == password);
                if (user == null)
                {
                    return Results.NotFound($"Credentials are incorrect");
                }
                dbContext.Users.Remove(user);
                dbContext.SaveChanges();
                return Results.NoContent();
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Results.Problem("An error occurred while processing your request to delete user.");
            }
        });
        
        return group;
    }
}