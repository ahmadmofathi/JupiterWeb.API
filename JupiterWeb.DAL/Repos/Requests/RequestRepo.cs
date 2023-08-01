using JupiterWeb.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.DAL
{
    public class RequestRepo : IRequestRepo
    {
        private readonly AppDbContext _context;

        public RequestRepo(AppDbContext context)
        {
            _context = context;
        }
        public string Add(Request req)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Request req)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Request> GetAll()
        {
            return _context.Set<Request>().ToList();
        }

        public Request? GetTaskById(string reqId)
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public bool Update(Request req)
        {
            throw new NotImplementedException();
        }
    }
}
