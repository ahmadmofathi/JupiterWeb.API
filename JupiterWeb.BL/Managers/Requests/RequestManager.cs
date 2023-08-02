using JupiterWeb.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.BL
{
    public class RequestManager : IRequestManager
    {
        private readonly IRequestRepo _requestRepo;

        public RequestManager(IRequestRepo requestRepo)
        {
            _requestRepo = requestRepo;
        }
        public IEnumerable<RequestReadDTO> GetAllRequests()
        {
            IEnumerable<Request> requestsFromDB = _requestRepo.GetAll();
            return requestsFromDB.Select(t => new RequestReadDTO
            {
                Name = t.Name,
                Comments = t.Comments,
                TaskID = t.TaskID,
                userSentById = t.userSentById,
                userSentToId = t.userSentToId,
            });
        }

        public RequestReadDTO? GetRequestById(string id)
        {
            throw new NotImplementedException();
        }
        public string Add(RequestAddDTO req)
        {
            Request request = new Request
            {
                Name= req.Name,
                Comments = req.Comments,
                TaskID = req.TaskID,
                userSentToId= req.userSentToId,
                userSentById= req.userSentById,
            };
            _requestRepo.Add(request);
            _requestRepo.SaveChanges();
            if (request.Id == null)
            {
                return "Not Found";
            }
            return request.Id;
        }

        public Task Create(Request req)
        {
            _requestRepo.Add(req);
            _requestRepo.SaveChanges();
            return Task.FromResult(req);
        }

        public bool Delete(string id)
        {
            Request? request = _requestRepo.GetReqById(id);
            if (request is null)
            {
                return false;
            }
            _requestRepo.Delete(request);
            _requestRepo.SaveChanges();
            return true;
        }

        public bool Update(RequestUpdateDTO req)
        {
            if (req.Id == null)
            {
                return false;
            }
            Request? request = _requestRepo.GetReqById(req.Id);
            if (request == null)
            {
                return false;
            }
            request.Name = req.Name;
            request.TaskID =  req.TaskID;
            request.userSentToId = req.userSentToId;
            request.userSentById = req.userSentById;
            request.IsApproved = req.IsApproved;
            request.IsReviewed = req.IsReviewed;
            request.Id = req.Id;
            _requestRepo.Update(request);
            _requestRepo.SaveChanges();
            return true;
        }
    }
}
