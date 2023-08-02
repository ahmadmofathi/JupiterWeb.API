using JupiterWeb.API.Data;
using Microsoft.AspNetCore.Identity;
using JupiterWeb.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using JupiterWeb.DAL;
using System.Xml.Linq;

namespace JupiterWeb.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITaskManager _taskManager;
        private readonly ITaskRepo _tasksRepo;
        private readonly IRequestRepo _requestRepo;

        public UserController(AppDbContext context, UserManager<User> userManager, IConfiguration config,IRequestRepo requestRepo, ITaskManager taskManager, ITaskRepo tasksRepo)
        {
            _context = context;
            _userManager = userManager;
            _configuration = config;
            _taskManager = taskManager;
            _tasksRepo = tasksRepo;
            _requestRepo = requestRepo;
        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<string>> Register(RegisterDTO registerDTO)
        {
            try
            {
                var newUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = registerDTO.Email,
                    UserName = registerDTO.UserName,
                    GetEmployedAt = registerDTO.GetEmployedAt,
                    Role = registerDTO.Role,
                    Address = registerDTO.Address,
                    Branch = registerDTO.Branch,
                    WhatsApp = registerDTO.WhatsApp,
                    PhoneNumber = registerDTO.PhoneNumber,
                    Name = registerDTO.Name,
                };

                var CreationResult = await _userManager.CreateAsync(newUser, registerDTO.Password);
                if (!CreationResult.Succeeded)
                {
                    return BadRequest(CreationResult.Errors);
                }
                // Fetch the user from the database
                var createdUser = await _userManager.FindByEmailAsync(newUser.Email);
                if (createdUser == null)
                {
                    return BadRequest("User was not found after creation");
                }

                var userClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, newUser.UserName),
                            new Claim(ClaimTypes.Email, newUser.Email),
                            new Claim(ClaimTypes.Role, newUser.Role),
                            new Claim(ClaimTypes.Country, "EGY"),
                            new Claim(ClaimTypes.DateOfBirth, newUser.GetEmployedAt.ToString("yyyy-MM-dd")),
                            new Claim(ClaimTypes.StreetAddress, newUser.Address),
                            new Claim(ClaimTypes.StateOrProvince, newUser.Branch),
                            new Claim(ClaimTypes.MobilePhone, newUser.PhoneNumber),
                        };


                // Save the user to the database
                // await _context.SaveChangesAsync();
                // Now add the claims
                var claimsResult = await _userManager.AddClaimsAsync(newUser, userClaims);
                if (!claimsResult.Succeeded)
                {
                    // If adding the claims fails, delete the user to avoid orphaned users
                    await _userManager.DeleteAsync(newUser);
                    return BadRequest(claimsResult.Errors);
                }
                /* var CreationResult = await _userManager.CreateAsync(newUser, registerDTO.Password);
                 if(!CreationResult.Succeeded)
                 {
                     return BadRequest(CreationResult.Errors);
                 }
                 await _userManager.AddClaimsAsync(newUser, userClaims);
                  */
                return Ok("Done");
            }
            catch (DbUpdateException ex)
            {
                var exceptionDetails = ex.InnerException?.Message;
                Console.WriteLine($"DbUpdateException: {exceptionDetails}");

                return BadRequest(exceptionDetails);
            }
            catch (Exception ex)
            {
                // Log the exception or return it
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<TokenDTO>> Login(LoginDTO credintials)
        {
            var user = await _userManager.FindByNameAsync(credintials.UserName);
            if (user == null)
            {
                return BadRequest("User Not Found");
            }
            if (await _userManager.IsLockedOutAsync(user))
            {
                return BadRequest("Try Again");
            }
            var userClaims = await _userManager.GetClaimsAsync(user);
            bool isAuthenticated = await _userManager.CheckPasswordAsync(user, credintials.Password);
            if (!isAuthenticated)
            {
                await _userManager.AccessFailedAsync(user);
                return Unauthorized("Wrong Credentials");
            }
            var exp = DateTime.Now.AddMinutes(30);
            var secretKey = _configuration.GetValue<string>("SecretKey");
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("The secret key cannot be null or empty.");
            }
            var secretKeyBytes = Encoding.ASCII.GetBytes(secretKey);
            var Key = new SymmetricSecurityKey(secretKeyBytes);
            var methodGeneratingToken = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256Signature);
            var jwt = new JwtSecurityToken(
                claims: userClaims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(30)
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            string tokenString = tokenHandler.WriteToken(jwt);
            return new TokenDTO
            {
                Token = tokenString,
                ExpiryDate = exp,
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                // Handle the case where password is null or empty, perhaps by returning an error.
                return BadRequest("The password cannot be null or empty.");
            }
            var result = await _userManager.CreateAsync(user, user.PasswordHash);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var existingUser = await _userManager.FindByIdAsync(id.ToString());

            if (existingUser == null)
            {
                return NotFound();
            }

            // Update properties
            existingUser.Email = user.Email;
            existingUser.Name = user.Name;
            existingUser.Role = user.Role;
            existingUser.Branch = user.Branch;
            existingUser.BasicSalary = user.BasicSalary;
            existingUser.WhatsApp = user.WhatsApp;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.UserName = user.UserName;

            var result = await _userManager.UpdateAsync(existingUser);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("tasks")]
        public async Task<IActionResult> CreateTaskAsync([FromBody] TaskAddDTO taskDTO)
        {
            // Parse the task data from the JSON payload
            JupiterTask task = new JupiterTask
            {
                Name = taskDTO.Name,
                AssignedById = taskDTO.AssignedById,
                AssignedToId = taskDTO.AssignedToId,
                Deadline = taskDTO.Deadline,
                Description = taskDTO.Description,
                IsDone = taskDTO.IsDone,
                Link = taskDTO.Link,
                TaskPoints = taskDTO.TaskPoints
            };

            // Add the task to the database
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // Return a response to the user
            return Ok(new
            {
                message = "Task created successfully",
                taskId = task.Id
            });
        }

        [HttpPut("{id}/submit")]
        public async Task<IActionResult> SubmitTask(string id, [FromBody] Submission request)
        {
            // Find the user in the database
            var user = await _userManager.FindByIdAsync(id);


            if (user == null)
            {
                return NotFound();
            }

            // Find the task in the database
            var task = await _context.Tasks.FindAsync(request.TaskId);

            if (task == null)
            {
                return NotFound();
            }

            // Check if the user is assigned to the task
            if (task.AssignedToId != user.Id)
            {
                return BadRequest("This task is not assigned to you.");
            }

            // Check if the task is already marked as done
            if (task.IsDone)
            {
                return BadRequest("This task is already marked as done.");
            }

            // Mark the task as done and increment the attempts
            task.ReviewRequested = true;
            task.Attempts++;

            // Create a new request
            var taskRequest = new Request
            {
                Name = task.Name,
                IsApproved = false,
                IsReviewed = false,
                Comments = new List<string>(),
                JupiterTask = task,
                UserSentBy = task.UserAssignedTo,
                userSentToId = request.userSentTo
            };

            // If the user assignedBy is submitting the task, add a comment to the task with the link of the submission
            if (task.AssignedById != user.Id)
            {
                var comment = $"Task submitted by {user.UserName}: {request.Comment}";
                taskRequest.Comments?.Add(comment);
            }

            // Add the review request to the database
            _context.Requests.Add(taskRequest);

            // Add the request to the collection of requests associated with the task
            task.Requests?.Add(taskRequest);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok("Task submitted successfully.");
        }
        [HttpGet("{id}/requests")]
        public async Task<IActionResult> GetRequests(string id)
        {
            var requests = await _context.Requests
                .Where(r => r.userSentToId == id)
                .ToListAsync();
            return Ok(requests);
        }


    }
}
