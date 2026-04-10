namespace Dtos {
public class UserPublicDto {
    public int Id { get; set; }

    public string Username { get; set; } = String.Empty;

    public string Biography { get; set; } = String.Empty;

    public DateTime JoinedOn { get; set; } = DateTime.Now;
}
}
