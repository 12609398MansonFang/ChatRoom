namespace ChatRoom.Api.Entities;

public class Message 
{
    public int MessageId { get; set; }
    
    public int MessageUserId { get; set; }
    public string? MessageText { get; set; }
}

