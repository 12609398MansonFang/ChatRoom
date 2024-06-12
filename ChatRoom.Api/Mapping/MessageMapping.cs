using ChatRoom.Api.Dtos;
using ChatRoom.Api.Entities;

namespace ChatRoom.Api.Mapping;

public static class MessageMapping
{
    public static MessageDto ToDto(this Message message)
    {    
        return new MessageDto(message.MessageId, message.MessageText);
    }


}
