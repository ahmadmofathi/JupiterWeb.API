using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.BL
{
    public class RequestAddDTO
    {
        public string? Name { get; set; }
        public List<string>? Comments { get; set; }
        public string? TaskID { get; set; }
        public string? userSentById { get; set; }
        public string? userSentToId { get; set; }
    }
}
