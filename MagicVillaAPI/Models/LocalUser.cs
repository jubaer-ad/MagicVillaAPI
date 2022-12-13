using System.ComponentModel.DataAnnotations;

namespace MagicVillaAPI.Models
{
	public class LocalUser
	{
		public int Id { get; set; }
		public string  UserName { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter password")]
		[MinLength(6, ErrorMessage = "Password must have minimum 6 characters")]
		public string Password { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
	}
}
