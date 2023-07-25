using JupiterWeb.API.Data;
using JupiterWeb.API.Data.Models;
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

namespace JupiterWeb.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public UserController(AppDbContext context,UserManager<User> userManager,IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _configuration= config;
        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<string>> Register (RegisterDTO registerDTO)
        {
            var newUser = new User
            {
                Email = registerDTO.Email,
                Password = registerDTO.Password,
                GetEmployedAt = registerDTO.GetEmployedAt,
                Role = registerDTO.Role,
                Address = registerDTO.Address,
                Branch = registerDTO.Branch,
                WhatsApp = registerDTO.WhatsApp,
                PhoneNumber = registerDTO.PhoneNumber,
                Name = registerDTO.Name,
            };
            var userClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, newUser.Name),
                            new Claim(ClaimTypes.Email, newUser.Email),
                            new Claim(ClaimTypes.Role, newUser.Role),
                            new Claim(ClaimTypes.Country, "EGY"),
                            new Claim(ClaimTypes.DateOfBirth, newUser.GetEmployedAt.ToString("yyyy-MM-dd")),
                            new Claim(ClaimTypes.StreetAddress, newUser.Address),
                            new Claim(ClaimTypes.StateOrProvince, newUser.Branch),
                            new Claim(ClaimTypes.MobilePhone, newUser.PhoneNumber),
                        };
            
            var CreationResult = await _userManager.CreateAsync(newUser, registerDTO.Password);
            if(!CreationResult.Succeeded)
            {
                return BadRequest(CreationResult.Errors);
            }
            await _userManager.AddClaimsAsync(newUser, userClaims);
            return Ok("Done");
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<TokenDTO>> Login(LoginDTO credintials)
        {
            var user = await _userManager.FindByNameAsync(credintials.Email);
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
                _userManager.AccessFailedAsync(user);
                return Unauthorized("Wrong Credentials");
            }
            var exp = DateTime.Now.AddMinutes(30);
            var secretKey = _configuration.GetValue<string>("SecretKey");
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
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
