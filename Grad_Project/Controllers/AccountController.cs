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

        public AccountController(UserManager<AppUser>userManager,IConfiguration configuration,DataContext db)
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
                    PhoneNumber = register.phone
                };
                IdentityResult result = await userManager.CreateAsync(user, register.password);
               
                if (result.Succeeded)
                {
                    var addCounter = new Counter()
                    {
                        id = register.counterId,
                        userId = user.Id
                    };
                    await db.counters.AddAsync(addCounter);
                    await db.SaveChangesAsync();
                    return Ok("Register");
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
                AppUser? user = await db.Users.Where(x=>x.idNumber==login.idNumber).FirstOrDefaultAsync();
                if (user != null)
                {
                    if (await userManager.CheckPasswordAsync(user, login.password))
                    {
                        var claims = new List<Claim>();
                        //claims.Add(new Claim("name", "value"));
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        var roles = await userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }
                        //signingCredentials
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                        var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            claims: claims,
                            issuer: configuration["JWT:Issuer"],
                            audience: configuration["JWT:Audience"],
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: sc
                            );
                        var _token = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                        };
                        var myCounter =await db.counters.Where(x => x.userId == user.Id).FirstOrDefaultAsync();
                        var data = new LoginResponseDto()
                        {
                            id = user.Id,
                            idNumber=user.idNumber,
                            name = user.UserName,
                            email = user.Email,
                            counterId=myCounter.id,
                            lastLogin=user.lastLogin,
                            token = _token.token,
                            expiration = _token.expiration

                        };
                        user.lastLogin = DateTime.Now;
                        await db.SaveChangesAsync();
                        return Ok(data);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User Name is invalid");
                }
            }
            return BadRequest(ModelState);
        }
    }
}
