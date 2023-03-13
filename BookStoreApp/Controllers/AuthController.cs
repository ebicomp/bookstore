using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Users;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly BookStoreDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> userManager;
        private readonly IConfiguration configuration;

        public AuthController(ILogger<AuthController> logger,
            BookStoreDbContext context,
            IMapper mapper,
            UserManager<ApiUser> userManager,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.configuration = configuration;
        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] UserDto userDto)
        {
            try
            {
                var user = mapper.Map<ApiUser>(userDto);
                user.UserName = userDto.Email;
                var result = await userManager.CreateAsync(user , userDto.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(userDto.UserRole))
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
                else
                {
                    await userManager.AddToRoleAsync(user, "Administrator");
                }
                return Accepted();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Register User {nameof(Register)}");
                return Problem(Messages.Error500Message);
            }
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginUserDto loginUserDto)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(loginUserDto.Email);
                if (user == null)
                    return Unauthorized();
                var passwordValid = await userManager.CheckPasswordAsync(user, loginUserDto.Password);
                if ( !passwordValid)
                {
                    return Unauthorized(loginUserDto);
                }
                var token = await GenerateToken(user);

                var authResponse = new AuthResponse
                {
                     Email = user.Email,
                     Token = token,
                     UserId = user.Id
                };
                return Accepted(authResponse);
            }
            catch (Exception ex)
            {
                logger.LogError(ex , $"Exception while login {nameof(Login)}");
                return Problem(Messages.Error500Message,statusCode: 500);
            }
        }

        private async Task<string> GenerateToken(ApiUser user)
        {

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();
            var userClaims = await userManager.GetClaimsAsync(user);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub ,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti ,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email ,user.Email),
                new Claim(CustomClaimType.Uid ,user.Id),
            }
            .Union(roleClaims)
            .Union(userClaims);

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(Convert.ToInt32(configuration["JwtSettings:Duration"])),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

