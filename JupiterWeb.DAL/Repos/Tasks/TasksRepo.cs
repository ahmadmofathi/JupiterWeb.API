using JupiterWeb.API.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.DAL
{
    public class TasksRepo : ITaskRepo
    {
        private readonly AppDbContext _context;

        public TasksRepo(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<JupiterTask> GetAllTasks()
        {
            return _context.Set<JupiterTask>().ToList();
        }

        public JupiterTask? GetTaskById(string taskId)
        {
            return _context.Set<JupiterTask>().Find(taskId);
        }
        public string Add(JupiterTask task)
        {
            if(task.Id == null)
            {
                return "Not Found";
            }      
            _context.Set<JupiterTask>().Add(task);

            return task.Id;
        }

        public bool Delete(JupiterTask task)
        {
            _context.Set<JupiterTask>().Remove(task);
            return true;
        }
        public bool Update(JupiterTask task)
        {
            _context.Set<JupiterTask>().Update(task);
            return true;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        
    }
}
