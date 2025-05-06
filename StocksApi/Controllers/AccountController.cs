using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StocksApi.Dtos.User;
using StocksApi.Models;
using StocksApi.Services.Interfaces;

namespace StocksApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (registerDto == null)
                    return BadRequest("Invalid registration data.");
                var user = new User
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (result.Succeeded)
                {
                    var roleRusult = await _userManager.AddToRoleAsync(user, "User");
                    if (roleRusult.Succeeded)
                        return Ok(
                            new NewUserDto
                            {
                                UserName = user.UserName,
                                Email = user.Email,
                                Token = _tokenService.CreateToken(user)
                            }
                        );
                    else
                    {
                        return StatusCode(500, roleRusult.Errors);
                    }
                }
                return StatusCode(500, result.Errors);
                //return BadRequest("User registration failed.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (loginDto == null)
                    return BadRequest("Invalid login data.");


                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);
                if (user == null)
                    return Unauthorized("Invalid Username.");

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (!result.Succeeded) return Unauthorized("Invalid Username and/or password.");

                return Ok(
                    new NewUserDto
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        Token = _tokenService.CreateToken(user)
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }
}
