using MagicVillaAPI.Logging;
using MagicVillaAPI.Repository.Implementation;
using MagicVillaAPI.Repository.IRepository;

namespace MagicVillaAPI
{
	public static class ServiceRegistration
	{
		public static void AddCustomeServices(this IServiceCollection services)
		{
			// Adding Custom logger to container for dependency injection
			services.AddSingleton<ILoggingCustom, LoggingCustomImpl>();
			services.AddScoped<IVillaRepository, VillaRepositoryImpl>();
			services.AddScoped<IVillaNumberRepository, VillaNumberRepositoryImpl>();
			services.AddScoped<IUserRepository, UserRepository>();
		}
	}
}
