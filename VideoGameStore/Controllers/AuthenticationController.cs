using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VideoGameStore.Data;
using VideoGameStore.Data.Static;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration,
            AppDbContext context, TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
            _tokenValidationParameters = tokenValidationParameters;
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

            var token = GenerateJwtToken(newUser, "user");

            return Ok(token);
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
                        var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, false, false);

                        if (result.Succeeded)
                        {
                            var roles = await _userManager.GetRolesAsync(user);

                            string role = "user";

                            foreach (string item in roles)
                            {
                                if (item.ToString() == "Admin")
                                {
                                    role = "Admin";
                                }
                            }

                            var jwtToken = await GenerateJwtToken(user, role);

                            return Ok(jwtToken);
                        }

                        return BadRequest(new AuthenticateResult()
                        {
                            Errors = new List<string>()
                            {"Your account has been locked. Please contact the helpdesk."},
                            Result = false
                        });
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

        private async Task<AuthenticateResult> GenerateJwtToken(ApplicationUser user, string role)
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
                    new Claim(type: JwtRegisteredClaimNames.Iat, value: DateTime.Now.ToUniversalTime().ToString()),
                    new Claim(ClaimTypes.Role, role)
                }),

                Expires = DateTime.Now.Add(TimeSpan.Parse(_configuration.GetSection(key: "JwtConfig:ExpiryTimeFrame").Value)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256),
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            //Refresh token info
            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                Token = RandomStringGenerator(36), // Generate a refresh token
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(3),
                IsRevoked = false,
                IsUsed = false,
                UserId = user.Id
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthenticateResult()
            {
                Result = true,
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
            };
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await VerifyAndGenerateToken(tokenRequest);

                if (result != null)
                {
                    return Ok(result);
                }
            }

            return BadRequest(new AuthenticateResult()
            {
                Errors = new List<string>()
                {
                    "Invalid parameters."
                },
                Result = false
            });
        }

        private async Task<AuthenticateResult> VerifyAndGenerateToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                _tokenValidationParameters.ValidateLifetime = true;

                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, 
                    _tokenValidationParameters, out var validatedToken);

                if(validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                        StringComparison.InvariantCultureIgnoreCase);

                    if(result == false)
                    {
                        return null;
                    }
                }

                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(
                    d => d.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                if( expiryDate < DateTime.UtcNow )
                {
                    return new AuthenticateResult()
                    {
                        Errors = new List<string>()
                        {
                            "Expired token."
                        },
                        Result = false
                    };
                }

                var  storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(n => n.Token == tokenRequest.RefreshToken);

                if (storedToken == null)
                {
                    return new AuthenticateResult()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid token."
                        },
                        Result = false
                    };
                }

                var jti = tokenInVerification.Claims.FirstOrDefault(n => n.Type == JwtRegisteredClaimNames.Jti).Value;

                if ( storedToken.IsUsed || storedToken.IsRevoked || storedToken.JwtId != jti)
                {
                    return new AuthenticateResult()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid token."
                        },
                        Result = false
                    };
                }

                if (storedToken.ExpiryDate < DateTime.UtcNow )
                {
                    return new AuthenticateResult()
                    {
                        Errors = new List<string>()
                        {
                            "Expired token."
                        },
                        Result = false
                    };
                }

                // Validation complete, can create new token.
                storedToken.IsUsed = true;
                _context.RefreshTokens.Update(storedToken);
                await _context.SaveChangesAsync();

                var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);

                var roles = await _userManager.GetRolesAsync(dbUser);

                string role = "user";

                foreach (string item in roles)
                {
                    if (item.ToString() == "Admin")
                    {
                        role = "Admin";
                    }
                }

                return await GenerateJwtToken(dbUser, role);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return new AuthenticateResult()
                {
                    Errors = new List<string>()
                        {
                            "Server Error."
                        },
                    Result = false
                };

            }
            
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0,0,0, DateTimeKind.Utc);

            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp);
            return dateTimeVal;
        }

        private string RandomStringGenerator(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_-";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
