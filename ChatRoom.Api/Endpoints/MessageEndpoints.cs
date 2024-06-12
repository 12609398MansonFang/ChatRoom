using ChatRoom.Api.Data;
using ChatRoom.Api.Dtos;
using ChatRoom.Api.Entities;
using ChatRoom.Api.Mapping;


namespace ChatRoom.Api.Endpoints;

public static class MessageEndpoints 
{
    public static RouteGroupBuilder MapMessageEndpoints(this WebApplication app){
        var group = app.MapGroup("messages");

        group.MapGet("/", (ChatRoomContext dbContext) => 
        {
            var messages = dbContext.Messages.ToList();
            var messageDtos = messages.Select(message => message.ToDto());
            return Results.Ok(messageDtos);
        });

        group.MapDelete("/{id}", (int id, ChatRoomContext dbContext) => 
        {
            var message = dbContext.Messages.Find(id);
            if (message == null)
            {
                return Results.NotFound();
            }

            dbContext.Messages.Remove(message);
            dbContext.SaveChanges();

            return Results.NoContent();
        });

        group.MapPost("/", (CreateMessageDto createMessageDto, ChatRoomContext dbContext) => 
        {
            try
            {
                if (createMessageDto == null || string.IsNullOrEmpty(createMessageDto.MessageText))
                {
                    return Results.BadRequest("Invalid message data.");
                }

                var message = new Message
                {
                    MessageText = createMessageDto.MessageText
                };

                dbContext.Messages.Add(message);
                dbContext.SaveChanges();

                var messageDto = message.ToDto();

                return Results.Created($"/messages/{message.MessageId}", messageDto);
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