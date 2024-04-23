using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository) : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager = userManager;
        private readonly ITokenRepository tokenRepository = tokenRepository;

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerDTO.Password);

            if (identityResult.Succeeded)
            {
                if (registerDTO.Roles != null && registerDTO.Roles.Length != 0)
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerDTO.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok(new { message = "User was registered! Please login." });
                    }
                }
                else
                {
                    return Ok(new { message = "User was registered! Please login." });
                }
            }

            return BadRequest(new { message = "Failed to register user." });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await userManager.FindByEmailAsync(loginDTO.Email);

            if (user != null)
            {
                var passwordIsValid = await userManager.CheckPasswordAsync(user, loginDTO.Password);

                if (passwordIsValid)
                {
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null && roles.Any())
                    {
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken,
                        };
                        return Ok(response);
                    }

                    return Unauthorized(new { message = "You are not authorized to make this request." });
                }
                else
                {
                    return BadRequest(new { message = "Email or password incorrect, please check your credentials." });
                }
            }

            return BadRequest(new { message = "Email or password incorrect, please check your credentials." });
        }
    }
}
