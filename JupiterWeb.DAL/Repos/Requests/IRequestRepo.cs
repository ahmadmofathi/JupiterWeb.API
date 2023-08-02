using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.DAL
{
    public interface IRequestRepo
    {
        IEnumerable<Request> GetAll();
       
        Request? GetReqById(string reqId);
        string Add(Request req);
        bool Delete(Request req);
        bool Update(Request req);
        int SaveChanges();
    }
}
