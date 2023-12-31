﻿using JupiterWeb.API.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.BL
{
    public class TaskReadDTO
    {
        
        public string? Name { get; set; }

        public bool IsDone { get; set; }

        public int TaskPoints { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public DateTime Deadline { get; set; }
        public string? AssignedById { get; set; }
        public User? UserAssignedBy { get; set; }
        public string? AssignedToId { get; set; }
        public User? UserAssignedTo { get; set; }
        public int Attempts { get; set; }

        public bool ReviewRequested { get; set; }
    }
}
