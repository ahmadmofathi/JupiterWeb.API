using JupiterWeb.DAL;
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
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int BasicSalary { get; set; }
        public int Bonus { get; set; }
        public string WhatsApp { get; set; } = string.Empty;
        public int JupiterCoins { get; set; }
        public DateTime GetEmployedAt { get; set; }
        public string Branch { get; set; } = string.Empty;
        public ICollection<JupiterTask>? AssignedByTasks { get; set; }
        public ICollection<JupiterTask>? AssignedToTasks { get; set; }

        public ICollection<Request>? Requests { get; set; }

    }
}