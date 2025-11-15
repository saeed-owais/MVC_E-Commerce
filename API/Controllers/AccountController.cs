using API.Response;
using BLL.DTOs.Account;
using DA.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.config = config;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail<RegisterDto>("Invalid input"));

            var existingEmail = await userManager.FindByEmailAsync(registerDto.Email);
            if (existingEmail != null)
                return Ok(ResponseHelper.Fail<RegisterDto>("This Email is already registered"));

            var existingPhone = userManager.Users.FirstOrDefault(u => u.PhoneNumber == registerDto.Phone);
            if (existingPhone != null)
                return Ok(ResponseHelper.Fail<RegisterDto>("This Phone is already registered"));

            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.Phone,
                Address = registerDto.Country
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return BadRequest(ResponseHelper.Fail<RegisterDto>(errors));
            }




            var userData = new
            {
                user.FullName,
                user.Email,
                user.PhoneNumber
            };

            return Ok(ResponseHelper.Success(userData, "User Registered Successfully"));
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail<LoginDto>("Invalid input"));

            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return BadRequest(ResponseHelper.Fail<LoginDto>("Email Is Incorrect"));

            var passwordValid = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!passwordValid)
                return BadRequest(ResponseHelper.Fail<LoginDto>("Password Is Incorrect"));

            var claims = new List<Claim>
             {
                   new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                   new Claim(ClaimTypes.NameIdentifier, user.Id),
                   new Claim(ClaimTypes.Name, user.Email)
             };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var roleName in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.Now.AddHours(2);

            var token = new JwtSecurityToken(
                issuer: config["JWT:IssuerIP"],
                audience: config["JWT:AudienceIP"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            var loginData = new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                user.FullName,
                user.Email,
                user.PhoneNumber
            };

            return Ok(ResponseHelper.Success(loginData, "Login successful"));
        }

        [HttpGet("profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(ResponseHelper.Fail<ProfileDto>("Invalid token"));

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(ResponseHelper.Fail<ProfileDto>("User not found"));

            var profileDto = new ProfileDto
            {
                Name = user.FullName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Status = user.IsDeleted ? "Disabled" : "Active",
                JoinDate = user.CreatedOnUtc,
                Address = user.Address
            };

            return Ok(ResponseHelper.Success(profileDto, "Profile retrieved successfully"));
        }

        [HttpPut("edit")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SaveEditProfile([FromBody] EditProfileDto editDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(ResponseHelper.Fail<EditProfileDto>("User not found"));

            if (editDto == null)
            {
                var currentData = new EditProfileDto
                {
                    Name = user.FullName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    Address = user.Address
                };
                return Ok(ResponseHelper.Success(currentData, "Current profile retrieved"));
            }

            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail<EditProfileDto>("Invalid input"));


            var emailExists = await userManager.Users.AnyAsync(u => u.Email == editDto.Email && u.Id != user.Id);
            if (emailExists)
                return BadRequest(ResponseHelper.Fail<EditProfileDto>("This Email is already registered"));

            var phoneExists = await userManager.Users.AnyAsync(u => u.PhoneNumber == editDto.Phone && u.Id != user.Id);
            if (phoneExists)
                return BadRequest(ResponseHelper.Fail<EditProfileDto>("This Phone is already registered"));


            user.FullName = editDto.Name;
            user.Email = editDto.Email;
            user.PhoneNumber = editDto.Phone;
            user.Address = editDto.Address;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return BadRequest(ResponseHelper.Fail<EditProfileDto>(errors));
            }

            var updatedData = new EditProfileDto
            {
                Name = user.FullName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Address = user.Address
            };

            return Ok(ResponseHelper.Success(updatedData, "Profile updated successfully"));
        }


        [HttpPut("change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail<ChangePasswordDto>("Invalid input"));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(ResponseHelper.Fail<ChangePasswordDto>("Invalid token"));

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(ResponseHelper.Fail<ChangePasswordDto>("User not found"));

            var result = await userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return BadRequest(ResponseHelper.Fail<ChangePasswordDto>(errors));
            }


            return Ok(ResponseHelper.Success<ChangePasswordDto>(null, "Password updated successfully"));
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok(ResponseHelper.Success<object>(null, "Logged out successfully"));
        }

    }

}
