using Dtos;
using Models;

namespace Mappers
{
    public static class UserMappers
    {
        public static UserMinimizedDto ToMinimizedDto(this User user)
        {
            return new UserMinimizedDto { Id = user.Id, Username = user.Username };
        }

        public static UserPublicDto ToPublicDto(this User user)
        {
            return new UserPublicDto
            {
                Id = user.Id,
                Username = user.Username,
                Biography = user.Biography,
                JoinedOn = user.JoinedOn
            };
        }

        public static UserPrivateDto ToPrivateDto(this User user)
        {
            return new UserPrivateDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Biography = user.Biography,
                JoinedOn = user.JoinedOn
            };
        }

        public static User ToUserFromCreate(this CreateUserRequestDto createDto)
        {
            return new User
            {
                Username = createDto.Username,
                Email = createDto.Email,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(createDto.Password)
            };
        }
    }
}
