using JupiterWeb.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.BL
{
    public interface IRequestManager
    {
        IEnumerable<RequestReadDTO> GetAllRequests();
        RequestReadDTO? GetRequestById(string id);
        string Add(RequestAddDTO req);
        bool Update(RequestUpdateDTO req);
        bool Delete(string id);
        Task Create(Request req);
    }
}
