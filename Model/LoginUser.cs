﻿using System.ComponentModel.DataAnnotations;

namespace AssetManagementWebApplication.Model
{
	public class LoginUser
	{
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress]
		public required string Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		public required string Password { get; set; }
	}
}

