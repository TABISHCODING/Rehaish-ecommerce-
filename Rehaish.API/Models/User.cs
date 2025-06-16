namespace MyntraClone.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = ""; // Removed required keyword
        public string Role { get; set; } = "Customer";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Added fields for profile update
        public string? Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
        public virtual Cart Cart { get; set; } = null!;
        public virtual Wishlist Wishlist { get; set; } = null!;
    }
}
