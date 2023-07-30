using JupiterWeb.API.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.BL
{
    public class TaskAddDTO
    {
        public string? Name { get; set; }

        public bool IsDone { get; set; }

        public int TaskPoints { get; set; }

        public string? Description { get; set; }
        public string? Link { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Deadline { get; set; }

        [ForeignKey("UserAssignedBy")]
        public string? AssignedById { get; set; }

        [ForeignKey("UserAssignedTo")]
        public string? AssignedToId { get; set; }
        public int Attempts { get; set; }

        public bool ReviewRequested { get; set; }

    }
}
