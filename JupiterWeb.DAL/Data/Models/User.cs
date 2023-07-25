using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace JupiterWeb.API.Data
{
    public class User : IdentityUser<string>
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public override int Id { get; set; }

        //public override string UserName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int BasicSalary { get; set; }
        public int Bonus { get; set; }

        //[Required]
       // [EmailAddress]
       // public string Email { get; set; } = string.Empty;

        //[Required]
       // [NotNull]
       // public string Password { get; set; }
        public string WhatsApp { get; set; } = string.Empty;

       // [Required]
        //[NotNull]
       // public string PhoneNumber { get; set; }

        public int JupiterCoins { get; set; }
        public DateTime GetEmployedAt { get; set; }
        public string Branch { get; set; } = string.Empty;
    }
}