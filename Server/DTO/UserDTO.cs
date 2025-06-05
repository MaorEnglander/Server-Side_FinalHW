namespace MyServer.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }
    }
}
