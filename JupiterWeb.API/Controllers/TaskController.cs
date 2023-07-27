using JupiterWeb.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JupiterWeb.DAL;
using JupiterWeb.BL;

namespace JupiterWeb.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private List<JupiterTask> _tasks = new List<JupiterTask> {
                new JupiterTask
                {
                    Id = "1",
                    Name = "Task 1",
                    IsDone = false,
                    TaskPoints = 10,
                    Description = "Description of Task 1",
                    Link = "https://example.com/task1",
                    Deadline = DateTime.Now.AddDays(7),
                    AssignedById = "1",
                    AssignedToId = "2"
                },
                new JupiterTask
                {
                    Id="2",
                    Name = "Task 2",
                    IsDone = true,
                    TaskPoints = 20,
                    Description = "Description of Task 2",
                    Link = "https://example.com/task2",
                    Deadline = DateTime.Now.AddDays(14),
                    AssignedById = "2",
                    AssignedToId = "1"
                },
                new JupiterTask
                {
                    Id="3",
                    Name = "Task 3",
                    IsDone = false,
                    TaskPoints = 30,
                    Description = "Description of Task 3",
                    Link = "https://example.com/task3",
                    Deadline = DateTime.Now.AddDays(21),
                    AssignedById = "3",
                    AssignedToId = "1"
                }
            };
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ITaskManager _taskManager;

        public TaskController(AppDbContext context, IConfiguration config, ITaskManager taskManager)
        {
            _context = context;
            _configuration = config;
            _taskManager = taskManager;
        
        }
        [HttpGet]
        public ActionResult<List<JupiterTask>> GetTasks()
        {
            if (!_tasks.Any())
            {
                return NotFound();
            }
            return Ok(_tasks);
        }
        [HttpGet]
        [Route("{id}")]
        public ActionResult<JupiterTask> GetTaskById(string id) { 
            JupiterTask? task = _tasks.FirstOrDefault(t => t.Id == id);
            if(task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }
        [HttpPost]
        public ActionResult AddTask(JupiterTask task)
        {
            _tasks.Add(task);
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult Update(JupiterTask task,string id)
        {
            if(id != task.Id)
            {
                return BadRequest();
            }
            JupiterTask? TaskToUpdate = _tasks.FirstOrDefault(x=>x.Id == id);
            if (TaskToUpdate == null)
            {
                return NotFound();
            }
            TaskToUpdate.Name = task.Name;
            TaskToUpdate.Description = task.Description;
            TaskToUpdate.IsDone = task.IsDone;
            TaskToUpdate.Deadline = task.Deadline;
            TaskToUpdate.Link = task.Link;
            TaskToUpdate.TaskPoints = task.TaskPoints;
            TaskToUpdate.AssignedToId = task.AssignedToId;
            TaskToUpdate.AssignedById = task.AssignedById;
            TaskToUpdate.UserAssignedBy = task.UserAssignedBy;
            TaskToUpdate.UserAssignedTo = task.UserAssignedTo;
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(string id) { 
            JupiterTask? taskDelete = _tasks.FirstOrDefault(x => x.Id == id);
            if (taskDelete == null)
            {
                return NotFound();
            }
            _tasks.Remove(taskDelete);
            return Ok();
        }
    }
}
