using AssetManagementWebApplication.Dto;
using AssetManagementWebApplication.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementWebApplication.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly AssetManagementContext dbContext;
		public UserController(AssetManagementContext dbContext)
		{
			this.dbContext = dbContext;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllUsers()
		{
			await dbContext.SaveChangesAsync();
			return Ok(dbContext.Users.ToList());
		}
		[HttpPost]
		public async Task<IActionResult> AddUser(AddUserDto adduserDto)
		{
			var newUser = new User
			{
				UserType = adduserDto.UserType,
				Name = adduserDto.Name,
				Email = adduserDto.Email,
				Password = adduserDto.Password,
				ContactNumber = adduserDto.ContactNumber,
				Address = adduserDto.Address,
				DateCreated = adduserDto.DateCreated,

			};
			dbContext.Users.Add(newUser);
			await dbContext.SaveChangesAsync();

			return Ok(newUser);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetUserById(int id)
		{
			var user = dbContext.Users.Find(id);
			if (user == null)
			{
				return NotFound();
			}
			await dbContext.SaveChangesAsync();
			return Ok(user);
		}
		[HttpPut]
		public async Task<IActionResult> UpdateUserById(int id, UpdateUserDto updateUserDto)
		{
			var user = dbContext.Users.Find(id);
			if (user == null) { return NotFound(); }

			user.UserType = updateUserDto.UserType;
			user.Name = updateUserDto.Name;
			user.Email = updateUserDto.Email;
			user.Password = updateUserDto.Password;
			user.ContactNumber = updateUserDto.ContactNumber;
			user.Address = updateUserDto.Address;
			user.DateCreated = updateUserDto.DateCreated;


			dbContext.Users.Update(user);
			await dbContext.SaveChangesAsync();
			dbContext.SaveChanges();
			return Ok(user);
		}
		[HttpDelete]
		public async Task<IActionResult> DeleteUserById(int id)
		{
			var user = dbContext.Users.Find(id);
			if (user == null)
			{
				return NotFound();
			}
			dbContext.Users.Remove(user);
			await dbContext.SaveChangesAsync();
			dbContext.SaveChanges();
			return Ok(user);
		}
	}
}
