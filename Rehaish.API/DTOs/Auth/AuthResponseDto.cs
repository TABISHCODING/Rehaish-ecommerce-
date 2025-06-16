namespace MyntraClone.API.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public string? Phone { get; set; } // Added
        public string? Gender { get; set; } // Added
        public DateTime? DateOfBirth { get; set; } // Added
    }
}
