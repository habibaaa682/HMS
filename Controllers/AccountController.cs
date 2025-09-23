using HMS.Models;
using HMS.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Guest> _userManager;

        public IConfiguration Configuration;

        public AccountController(UserManager<Guest> userManager,IConfiguration configuration) {
            _userManager = userManager;
            Configuration = configuration;
        }
    
        [HttpPost]
        public async Task<IActionResult> Register(GuestDto guestDto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var guest = new Guest {
                FirstName = guestDto.FirstName,
                LastName = guestDto.LastName,
                UserName = guestDto.UserName,
                Email = guestDto.Email,
                PhoneNumber = guestDto.PhoneNumber
            };
            var result = await _userManager.CreateAsync(guest, guestDto.Password);
            if (result.Succeeded) {
                return Ok("User registered successfully");
            }
            return BadRequest(result.Errors);
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto) {
            if (ModelState.IsValid)
            {
                Guest guest = await _userManager.FindByNameAsync(loginDto.UserName);
                if (guest != null && await _userManager.CheckPasswordAsync(guest, loginDto.Password))
                {
                    var claims =new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, guest.UserName));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, guest.Id));
                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                    var roles = await _userManager.GetRolesAsync(guest);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                    }

                    var token = new JwtSecurityToken(
                        issuer: Configuration["Jwt:Issuer"],
                        audience: Configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(
                            new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                            Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256)
                        );
                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
                else
                {
                    return Unauthorized("Invalid username or password");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
