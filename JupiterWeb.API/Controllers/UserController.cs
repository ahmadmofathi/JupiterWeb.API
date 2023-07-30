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


        public UserController(AppDbContext context,UserManager<User> userManager,IConfiguration config,ITaskManager taskManager)
        {
            _context = context;
            _userManager = userManager;
            _configuration= config;
            _taskManager = taskManager;
        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<string>> Register(RegisterDTO registerDTO)
        {
            try { 
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
            catch (Exception ex){
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
            if(await _userManager.IsLockedOutAsync(user))
            {
                return BadRequest("Try Again");
            }
            var userClaims= await _userManager.GetClaimsAsync(user);
            bool isAuthenticated = await _userManager.CheckPasswordAsync(user,credintials.Password);
            if(!isAuthenticated)
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

        [HttpPost("createTask")]
        public async Task<IActionResult> CreateTask([FromBody] TaskAddDTO createTaskDto)
        {
            var userAssignedBy = await _userManager.FindByIdAsync(createTaskDto.AssignedById);

            if (userAssignedBy == null)
            {
                return BadRequest("Invalid userAssignedBy id");
            }

            var task = new JupiterTask
            {
                Description = createTaskDto.Description,
                AssignedById = createTaskDto.AssignedById,
                AssignedToId = createTaskDto.AssignedToId,
                Attempts = 0,
                ReviewRequested = false
            };

            await _taskManager.Create(task);

            return Ok();
        }
    }
}
