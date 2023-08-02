using JupiterWeb.API.Data;
using JupiterWeb.BL;
using JupiterWeb.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JupiterWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IRequestManager _requestManager;


        public RequestsController(AppDbContext context, IConfiguration configuration, IRequestManager requestManager)
        {
            _context = context;
            _configuration = configuration;
            _requestManager = requestManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<Request>>> GetAllRequests()
        {
            var reqs = await _context.Requests.ToListAsync();

            if (!reqs.Any())
            {
                return NotFound();
            }

            return Ok(reqs);
        }




    }
}
