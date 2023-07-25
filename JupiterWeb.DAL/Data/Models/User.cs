using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace JupiterWeb.API.Data.Models
{
    public class User : IdentityUser
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public int BasicSalary { get; set; }
            public int Bonus { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
            [Required]
            [NotNull]
            public string Password { get; set; }
            public string WhatsApp { get; set; } = string.Empty;
            [NotNull]
            public string PhoneNumber { get; set; }
            public int JupiterCoins { get; set; }
            public DateTime GetEmployedAt { get; set; }
            public string Branch { get; set; } = string.Empty;
        }
    }

