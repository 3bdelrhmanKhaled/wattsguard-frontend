using Grad_Project.Database;
using Grad_Project.DTO;
using Grad_Project.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Grad_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration configuration;
        private readonly DataContext db;

        public AccountController(UserManager<AppUser> userManager, IConfiguration configuration, DataContext db)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.db = db;
        }

        [HttpPost("RegisterAsUser")]
        public async Task<IActionResult> RegisterAsUser(RegisterDto register)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    idNumber = register.idNumber,
                    UserName = register.name,
                    Email = register.email,
                    PhoneNumber = register.phone,
                    lastLogin = DateTime.UtcNow
                };

                IdentityResult result = await userManager.CreateAsync(user, register.password);

                if (result.Succeeded)
                {
                    var addCounter = new Counter()
                    {
                        id = Guid.NewGuid().ToString(),
                        CounterId = register.counterId,
                        userId = user.Id
                    };
                    await db.counters.AddAsync(addCounter);

                    var address = new Address
                    {
                        UserId = user.Id,
                        Street = register.address.Street, 
                        Region = register.address.Region, 
                        City = register.address.City, 
                        Governorate = register.address.Governorate 
                    };
                    await db.addresses.AddAsync(address);

                    await db.SaveChangesAsync();
                    return Ok("User registered successfully");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(LoginDto login)
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await db.Users
                    .Where(x => x.idNumber == login.idNumber)
                    .FirstOrDefaultAsync();
                if (user != null)
                {
                    if (await userManager.CheckPasswordAsync(user, login.password))
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        var roles = await userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                        var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            claims: claims,
                            issuer: configuration["Jwt:Issuer"],
                            audience: configuration["Jwt:Audience"],
                            expires: DateTime.UtcNow.AddHours(1),
                            signingCredentials: sc
                        );
                        var _token = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        var myCounter = await db.counters
                            .Where(x => x.userId == user.Id)
                            .FirstOrDefaultAsync();

                        var userAddress = await db.addresses
                            .Where(a => a.UserId == user.Id)
                            .FirstOrDefaultAsync();

                        user.lastLogin = DateTime.UtcNow;

                        var data = new LoginResponseDto()
                        {
                            id = user.Id,
                            idNumber = user.idNumber,
                            name = user.UserName,
                            email = user.Email,
                            counterId = myCounter?.CounterId,
                            lastLogin = user.lastLogin,
                            token = _token.token,
                            expiration = _token.expiration,
                            address = userAddress != null ? new AddressDto
                            {
                                Street = userAddress.Street,
                                Region = userAddress.Region,
                                City = userAddress.City,
                                Governorate = userAddress.Governorate
                            } : null
                        };

                        await db.SaveChangesAsync();
                        return Ok(data);
                    }
                    else
                    {
                        return Unauthorized("Invalid password");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User with this ID number does not exist");
                }
            }
            return BadRequest(ModelState);
        }
    }
}