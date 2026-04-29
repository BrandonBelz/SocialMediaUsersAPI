namespace Dtos
{
    public class UsersQueryDto
    {
        public string? Username { get; set; }
        public bool ExactMatch { get; set; } = false;
    }
}
