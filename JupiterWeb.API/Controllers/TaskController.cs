﻿using JupiterWeb.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JupiterWeb.DAL;
using JupiterWeb.BL;
using Microsoft.EntityFrameworkCore;

namespace JupiterWeb.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ITaskManager _taskManager;
        private readonly UserManager<User> _userManager;

        public TaskController(AppDbContext context, IConfiguration configuration, ITaskManager taskManager, UserManager<User> userManager)
        {
            _context = context;
            _configuration = configuration;
            _taskManager = taskManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<JupiterTask>>> GetTasksAsync()
        {
            var tasks = await _context.Tasks.ToListAsync();

            if (!tasks.Any())
            {
                return NotFound();
            }

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JupiterTask>> GetTaskByIdAsync(string id)
        {
            var task = await _context.Tasks.SingleOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult> AddTaskAsync(JupiterTask task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(string id, JupiterTask task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            var taskToUpdate = await _context.Tasks.FindAsync(id);

            if (taskToUpdate == null)
            {
                return NotFound();
            }

            // update properties
            taskToUpdate.Name = task.Name;
            taskToUpdate.Description = task.Description;
            taskToUpdate.IsDone = task.IsDone;
            taskToUpdate.Deadline = task.Deadline;
            taskToUpdate.Link = task.Link;
            taskToUpdate.TaskPoints = task.TaskPoints;
            taskToUpdate.AssignedToId = task.AssignedToId;
            taskToUpdate.AssignedById = task.AssignedById;
            taskToUpdate.UserAssignedBy = task.UserAssignedBy;
            taskToUpdate.UserAssignedTo = task.UserAssignedTo;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            var taskToDelete = await _context.Tasks.FindAsync(id);

            if (taskToDelete == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(taskToDelete);
            await _context.SaveChangesAsync();

            return Ok();
        }

       
    }
}