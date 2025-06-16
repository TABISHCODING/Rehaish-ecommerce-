﻿using System.ComponentModel.DataAnnotations;

namespace MyntraClone.API.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string AddressLine1 { get; set; } = string.Empty;

        public string? AddressLine2 { get; set; }

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Postal code must be 6 digits")]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string Phone { get; set; } = string.Empty;

        public bool IsDefault { get; set; } = false;

        // Navigation property
        public virtual User User { get; set; } = null!;
    }
}
