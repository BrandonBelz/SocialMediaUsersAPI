using Dtos;
using Models;

namespace Mappers
{
    public static class UserMappers
    {
        public static UserMinimizedDto ToMinimizedDto(this User user)
        {
            return new UserMinimizedDto
            {
                Id = user.Id,
                Username = user.Username,
                ProfilePicUrl = user.ProfilePicUrl,
            };
        }

        public static UserPublicDto ToPublicDto(this User user)
        {
            return new UserPublicDto
            {
                Id = user.Id,
                Username = user.Username,
                Biography = user.Biography,
                JoinedOn = user.JoinedOn,
                ProfilePicUrl = user.ProfilePicUrl,
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
                JoinedOn = user.JoinedOn,
                ProfilePicUrl = user.ProfilePicUrl,
            };
        }

        public static User ToUserFromCreate(this CreateUserRequestDto createDto)
        {
            return new User
            {
                Username = createDto.Username,
                Email = createDto.Email,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(createDto.Password),
            };
        }

        public static FriendshipMinimalDto ToFriendshipMinimalDto(this Friendship friendship)
        {
            return new FriendshipMinimalDto
            {
                User1Id = friendship.User1Id,
                User1 = friendship.User1.ToMinimizedDto(),
                User2Id = friendship.User2Id,
                User2 = friendship.User2.ToMinimizedDto(),
                FriendsSince = friendship.FriendsSince,
            };
        }

        public static FriendRequestMinimalDto ToFriendRequestMinimalDto(
            this FriendRequest friendRequest
        )
        {
            return new FriendRequestMinimalDto
            {
                RecipientId = friendRequest.RecipientId,
                Recipient = friendRequest.Recipient.ToMinimizedDto(),
                RequesterId = friendRequest.RequesterId,
                Requester = friendRequest.Requester.ToMinimizedDto(),
                SentAt = friendRequest.SentAt,
            };
        }

        public static ProfileDto ToProfileDto(this User user, int viewerId)
        {
            return new ProfileDto(user, viewerId);
        }

        public static JwtDto ToJwtDto(this string jwt)
        {
            return new JwtDto { JwtKey = jwt };
        }
    }
}
