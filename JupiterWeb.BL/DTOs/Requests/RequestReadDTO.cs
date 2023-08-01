using JupiterWeb.API.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JupiterWeb.DAL;

namespace JupiterWeb.BL
{
    public class RequestReadDTO
    {
        public string? Name { get; set; }
        public bool IsApproved { get; set; }
        public bool IsReviewed { get; set; }
        public List<string>? Comments { get; set; }
        public string? TaskID { get; set; }
        public string? userSentById { get; set; }
        public string? userSentToId { get; set; }
    }
}
