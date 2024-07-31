namespace Core.Application.Models
{
    public class LoginDto
    {
        public int Id { get; set; }

        public DateTime? Exp { get; set; }

        public string? Token { get; set; }
    }
}
