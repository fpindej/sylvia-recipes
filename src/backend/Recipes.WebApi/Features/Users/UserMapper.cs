using Recipes.Application.Features.Authentication.Dtos;
using Recipes.WebApi.Features.Users.Dtos;

namespace Recipes.WebApi.Features.Users;

public static class UserMapper
{
    public static UserResponse ToResponse(this UserOutput user) => new()
    {
        Id = user.Id,
        Username = user.UserName,
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName,
        PhoneNumber = user.PhoneNumber,
        Bio = user.Bio,
        AvatarUrl = user.AvatarUrl,
        Roles = user.Roles
    };
}
