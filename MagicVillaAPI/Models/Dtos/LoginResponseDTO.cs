namespace MagicVillaAPI.Models.Dtos
{
	public class LoginResponseDTO
	{
		public LocalUser? User { get; set; } = new LocalUser();
		public string Token { get; set; } = string.Empty;
	}
}
