using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.DAL
{
    public interface ITaskRepo
    {
        IEnumerable<JupiterTask> GetAllTasks();
        JupiterTask? GetTaskById(string taskId);
        string Add(JupiterTask task);
        bool Delete(JupiterTask task);
        bool Update(JupiterTask task);
        int SaveChanges();
    }
}
