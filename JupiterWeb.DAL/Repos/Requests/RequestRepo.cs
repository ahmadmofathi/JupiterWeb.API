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
            if (req.Id == null)
            {
                return "Not Found";
            }
            _context.Set<Request>().Add(req);

            return req.Id;
        }

        public bool Delete(Request req)
        {
            _context.Set<Request>().Remove(req);
            return true;
        }

        public IEnumerable<Request> GetAll()
        {
            return _context.Set<Request>().ToList();
        }

        public Request? GetReqById(string reqId)
        {
            return _context.Set<Request>().Find(reqId);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public bool Update(Request req)
        {
            _context.Set<Request>().Update(req);
            return true;
        }
    }
}
