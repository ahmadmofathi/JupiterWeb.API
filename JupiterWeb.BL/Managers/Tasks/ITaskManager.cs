using JupiterWeb.BL.DTOs.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.BL
{
    public interface ITaskManager
    {
        IEnumerable<TaskReadDTO> GetTasks();
        TaskReadDTO? GetTaskById(string id);
        string Add (TaskAddDTO task);
        bool Update(TaskUpdateDTO task);
        bool Delete(string id);
    }
}
