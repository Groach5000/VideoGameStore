using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Drawing.Printing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VideoGameStore.Configurations;
using VideoGameStore.Data;
using VideoGameStore.Data.Static;
using VideoGameStore.Data.ViewModels;
using VideoGameStore.Models;
using VideoGameStore.Models.DTOs;

namespace VideoGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // api/Authentication
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route(template: "Register")]
        public async Task<IActionResult> Register([FromBody] ApplicationUserRegistrationResquestDto requestDto)
        {
            // Validate incomming request
            if(!ModelState.IsValid)
            {
                return BadRequest(error: new AuthenticateResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid input"
                    }
                });
            }

            var user = await _userManager.FindByEmailAsync(requestDto.EmailAddress);

            if (user != null)
            {
                return BadRequest(error: new AuthenticateResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "This email address already has an associated account."
                    }
                });
            }

            var newUser = new ApplicationUser()
            {
                FullName = requestDto.FullName,
                Email = requestDto.EmailAddress,
                UserName = requestDto.EmailAddress
            };

            var newUserCreate = await _userManager.CreateAsync(newUser, requestDto.Password);

            if (newUserCreate.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            else if (newUserCreate.Errors != null)
            {
                List<string> errorList = new List<string>();

                foreach (var error in newUserCreate.Errors)
                {
                    errorList.Add(error.Description);
                }

                return BadRequest(error: new AuthenticateResult()
                {
                    Result = false,
                    Errors = errorList
                });
            }

            var token = GenerateJwtToken(newUser);

            return Ok(new AuthenticateResult()
            {
                Result = true,
                Token = token
            });
        }

        [HttpPost]
        [Route(template: "Login")]
        public async Task<IActionResult> Login([FromBody] ApplicationUserLoginResquestDto loginRequest)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginRequest.EmailAddress);

                if (user != null)
                {
                    var passwordCheck = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

                    if (passwordCheck)
                    {
                        var jwtToken = GenerateJwtToken(user);

                        return Ok(new AuthenticateResult() { Result = true, Token = jwtToken });
                    }
                }
            }

            return BadRequest(new AuthenticateResult()
            {
                Errors = new List<string>()
                {
                    "Could not verify credentials. Please try again."
                },
                Result = false
            });
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration.GetSection(key: "JwtConfig:Secret").Value);

            // Token descriptior
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(type: "Id", value: user.Id),
                    new Claim(type: JwtRegisteredClaimNames.Sub, value: user.Email),
                    new Claim(type: JwtRegisteredClaimNames.Email, value: user.Email),
                    new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                    new Claim(type: JwtRegisteredClaimNames.Iat, value: DateTime.Now.ToUniversalTime().ToString())
                }),

                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}
