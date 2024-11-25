using AssetManagementWebApplication.Dto;
using AssetManagementWebApplication.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AssetManagementWebApplication.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly AssetManagementContext _dbContext;
		private readonly IConfiguration _configuration;
		

		public UserController(AssetManagementContext dbContext, IConfiguration configuration)
		{
			_dbContext = dbContext;
			_configuration = configuration;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == registerUser.Email);
			if (existingUser != null)
				return BadRequest(new { message = "User with this email already exists." });

			var newUser = new User
			{
				UserType = registerUser.UserType,
				Name = registerUser.Name,
				Email = registerUser.Email,
				Password = BCrypt.Net.BCrypt.HashPassword(registerUser.Password),
				ContactNumber = registerUser.ContactNumber,
				Address = registerUser.Address,
				DateCreated = DateTime.UtcNow
			};

			_dbContext.Users.Add(newUser);
			await _dbContext.SaveChangesAsync();

			return Ok(new { message = "User registered successfully!", userId = newUser.UserId });
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginUser loginUser)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var user = _dbContext.Users.FirstOrDefault(u => u.Email == loginUser.Email);
			if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
				return Unauthorized(new { message = "Invalid email or password." });

			var token = GenerateJwtToken(user);

			return Ok(new { token, userId = user.UserId });
		}
		private string GenerateJwtToken(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
					new Claim(ClaimTypes.Name, user.Name),
					new Claim(ClaimTypes.Role, user.UserType)
				}),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
		[HttpGet]
		public IActionResult GetAllUsers()
		{
			var users = _dbContext.Users.Select(u => new
			{
				u.UserId,
				u.Name,
				u.Email,
				u.UserType,
				u.ContactNumber,
				u.Address,
				u.DateCreated
			}).ToList();

			return Ok(users);
		}

		[HttpGet("{id}")]
		public IActionResult GetUserById(int id)
		{
			var user = _dbContext.Users.Find(id);
			if (user == null)
				return NotFound();

			return Ok(new
			{
				user.UserId,
				user.Name,
				user.Email,
				user.UserType,
				user.ContactNumber,
				user.Address,
				user.DateCreated
			});
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUserById(int id, [FromBody] UpdateUserDto updateUserDto)
		{
			var user = _dbContext.Users.Find(id);
			if (user == null)
				return NotFound();

			user.UserType = updateUserDto.UserType;
			user.Name = updateUserDto.Name;
			user.Email = updateUserDto.Email;
			user.ContactNumber = updateUserDto.ContactNumber;
			user.Address = updateUserDto.Address;

			if (!string.IsNullOrWhiteSpace(updateUserDto.Password))
				user.Password = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);

			_dbContext.Users.Update(user);
			await _dbContext.SaveChangesAsync();

			return Ok(new { message = "User updated successfully!" });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUserById(int id)
		{
			var user = _dbContext.Users.Find(id);
			if (user == null)
				return NotFound();

			_dbContext.Users.Remove(user);
			await _dbContext.SaveChangesAsync();

			return Ok(new { message = "User deleted successfully!" });
		}

	}
}
