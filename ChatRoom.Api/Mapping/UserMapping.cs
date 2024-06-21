using ChatRoom.Api.Dtos;
using ChatRoom.Api.Entities;

namespace ChatRoom.Api.Mapping;

public static class UserMapping
{
    public static UserDto ToDto(this User user)
    {    
        return new UserDto(user.UserId, user.UserName, user.UserEmail, user.UserPassword);
    }
}