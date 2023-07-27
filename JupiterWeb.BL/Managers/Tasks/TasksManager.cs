using JupiterWeb.BL.DTOs.Tasks;
using JupiterWeb.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.BL
{
    public class TasksManager : ITaskManager
    {
        private readonly ITaskRepo _tasksRepo;

        public TasksManager(ITaskRepo taskRepo)
        {
            _tasksRepo = taskRepo;
        }
        public IEnumerable<TaskReadDTO> GetTasks()
        {
            IEnumerable<JupiterTask> tasksFromDB = _tasksRepo.GetAllTasks();
            return tasksFromDB.Select(t => new TaskReadDTO
            {
                Name = t.Name,
                AssignedById = t.AssignedById,
                AssignedToId = t.AssignedToId,
                UserAssignedBy = t.UserAssignedBy,
                UserAssignedTo = t.UserAssignedTo,
                Deadline = t.Deadline,
                Description = t.Description,
                IsDone = t.IsDone,
                Link = t.Link,
                TaskPoints = t.TaskPoints,
            });

        }
        public TaskReadDTO? GetTaskById(string id)
        {
            throw new NotImplementedException();
        }

        
        public string Add(TaskAddDTO taskFromReq)
        {
            JupiterTask task = new JupiterTask
            {
                Name = taskFromReq.Name,
                AssignedById = taskFromReq.AssignedById,
                AssignedToId = taskFromReq.AssignedToId,
                UserAssignedBy = taskFromReq.UserAssignedBy,
                UserAssignedTo = taskFromReq.UserAssignedTo,
                Deadline = taskFromReq.Deadline,
                Description = taskFromReq.Description,
                IsDone = taskFromReq.IsDone,
                Link = taskFromReq.Link,
                TaskPoints = taskFromReq.TaskPoints,
            };
            _tasksRepo.Add(task);
            _tasksRepo.SaveChanges();
            if (task.Id == null)
            {
                return "Not Found";
            }
            return task.Id;
        }

        public bool Delete(string id)
        {
            JupiterTask? task = _tasksRepo.GetTaskById(id);
            if (task is null) { 
                return false;
            }
            _tasksRepo.Delete(task);
            _tasksRepo.SaveChanges();
            return true;
        }

        public bool Update(TaskUpdateDTO taskFromReq)
        {
            if (taskFromReq.Id == null)
            {
                return false;
            }
            JupiterTask? task = _tasksRepo.GetTaskById(taskFromReq.Id);
            if(task == null)
            {
                return false;
            }
            task.Name = taskFromReq.Name;
            task.AssignedById = taskFromReq.AssignedById;
            task.AssignedToId = taskFromReq.AssignedToId;
            task.UserAssignedBy = taskFromReq.UserAssignedBy;
            task.UserAssignedTo = taskFromReq.UserAssignedTo;
            task.Deadline = taskFromReq.Deadline;
            task.Description = taskFromReq.Description;
            task.IsDone = taskFromReq.IsDone;
            task.Link = taskFromReq.Link;
            task.TaskPoints = taskFromReq.TaskPoints;
            _tasksRepo.Update(task);
            _tasksRepo.SaveChanges();
            return true;
        }
    }
}
