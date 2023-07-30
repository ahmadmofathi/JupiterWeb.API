using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JupiterWeb.API.Data;

namespace JupiterWeb.DAL
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        public string? Name { get; set; }
        bool isApproved { get; set; }
        public bool IsReviewed { get; set; }

        List<string>? Comments { get; set; }

        [ForeignKey("TaskID")]
        public string? TaskID { get; set; }
        public virtual JupiterTask? JupiterTask { get; set; }

        [ForeignKey("UserSentBy")]
        public string? userSentById { get; set; }
        public virtual User? UserSentBy { get; set; }

        [ForeignKey("UserSentTo")]
        public string? userSentToId { get; set; }
        public virtual User? UserSentTo { get; set; }
    }
}
