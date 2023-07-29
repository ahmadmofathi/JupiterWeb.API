using JupiterWeb.API.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.DAL
{
    public class JupiterTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        public string? Name { get; set; }

        public bool IsDone { get; set; }

        public int TaskPoints { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? Link { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Deadline { get; set; }

        [ForeignKey("UserAssignedBy")]
        public string? AssignedById { get; set; }
        public User? UserAssignedBy { get; set; }

        [ForeignKey("UserAssignedTo")]
        public string? AssignedToId { get; set; }
        public User? UserAssignedTo { get; set; }
    }
}
