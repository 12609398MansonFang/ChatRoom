using ChatRoom.Api.Data;
using ChatRoom.Api.Dtos;
using ChatRoom.Api.Entities;
using ChatRoom.Api.Mapping;


namespace ChatRoom.Api.Endpoints;

public static class MessageEndpoints 
{
    public static RouteGroupBuilder MapMessageEndpoints(this WebApplication app){
        var group = app.MapGroup("messages");

        // API to get All messages from the database
        group.MapGet("/all", (ChatRoomContext dbContext) => 
        {
            try{
                var messages = dbContext.Messages.ToList();
                var messageDto = messages.Select(message => message.ToDto());
                return Results.Ok(messageDto);

            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
                return Results.Problem("An error occurred while processing your request to get all messages.");
            }  
        });

        // API to get All messages specific to a room from the database
        group.MapGet("/room{roomId}", (int roomId, ChatRoomContext dbContext) => 
        {
            try{
                var messages = dbContext.Messages
                    .Where(m => m.MessageRoomId == roomId)
                    .ToList();
                var messageDto = messages.Select(message => message.ToDto());
                return Results.Ok(messageDto);
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
                return Results.Problem("An error occurred while processing your request to get all messages specific to a room.");
            }  
        });

        // API to Delete a Specific Message
        group.MapDelete("/delete{id}", (int id, ChatRoomContext dbContext) => 
        {
            try{
                var message = dbContext.Messages.Find(id);
                if (message == null){
                    return Results.NotFound();
                    }
                dbContext.Messages.Remove(message);
                dbContext.SaveChanges();
                return Results.NoContent();
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
                return Results.Problem("An error occurred while processing your request delete message.");
            }  
        });

        // API to create a Message
        group.MapPost("/create", (CreateMessageDto createMessageDto, ChatRoomContext dbContext) => 
        {
            try
            {
                if (createMessageDto == null || string.IsNullOrEmpty(createMessageDto.MessageText))
                {
                    return Results.BadRequest("Invalid message data.");
                }

                var message = new Message
                {
                    MessageUserId = createMessageDto.MessageUserId,
                    MessageRoomId = createMessageDto.MessageRoomId,
                    MessageText = createMessageDto.MessageText
                };

                dbContext.Messages.Add(message);
                dbContext.SaveChanges();

                var messageDto = message.ToDto();

                return Results.Created($"/messages/create/{message.MessageRoomId}", messageDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Results.Problem("An error occurred while processing your request to create this message.");
            }
        });
        
        return group;
    }
}