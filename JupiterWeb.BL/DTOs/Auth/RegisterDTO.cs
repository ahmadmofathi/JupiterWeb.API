using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.BL
{
    public record RegisterDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string WhatsApp { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime GetEmployedAt { get; set; }
        public string Branch { get; set; } = string.Empty;
    }
}
