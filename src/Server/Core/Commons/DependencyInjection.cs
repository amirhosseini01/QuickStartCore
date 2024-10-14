using FileSignatures;
using FileSignatures.Formats;
using Microsoft.AspNetCore.Identity;
using Server.Areas.Identity;
using Server.Core.Commons.UploadFile;
using Server.Core.Data;
using Server.Core.Data.Repositories.Contracts;
using Server.Core.Modules.User.Models;

namespace Server.Core.Commons;

public static class DependencyInjection
{
	private static IServiceCollection AddScrutorRepositories(this IServiceCollection services)
	{
		services.Scan(scan => scan
			.FromAssembliesOf(typeof(IGenericRepository<>))
				// repositories
				.AddClasses(classes => classes.AssignableTo(typeof(IGenericRepository<>)))
					.AsImplementedInterfaces()
					.WithScopedLifetime()
					// services
					// .AddClasses(classes => classes.AssignableTo(typeof(IGenericService)))
					//     .AsSelf()
					//     .WithScopedLifetime()
					);

		return services;
	}

	private static IServiceCollection AddFileFormatLocator(this IServiceCollection services)
	{
		var recognized = FileFormatLocator.GetFormats().OfType<Image>();
		var inspector = new FileFormatInspector(recognized);
		services.AddSingleton<IFileFormatInspector>(inspector);

		return services;
	}

	public static IServiceCollection AddCustomServices(this IServiceCollection services)
	{
		services.AddScrutorRepositories();

		services.AddScoped<FileUploader>();

		services.AddFileFormatLocator();

		return services;
	}

	public static IServiceCollection AddIdentityServices(this IServiceCollection services)
	{
		services.AddIdentity<AppUser, IdentityRole<int>>(options =>
			{
				options.SignIn.RequireConfirmedPhoneNumber = true;
				options.SignIn.RequireConfirmedEmail = false;
			})
			.AddEntityFrameworkStores<BaseDbContext>()
			.AddErrorDescriber<CustomIdentityErrorDescriber>();
		services.Configure<IdentityOptions>(options =>
		{
			// Password settings.
			options.Password.RequireDigit = false;
			options.Password.RequireLowercase = false;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireUppercase = false;
			options.Password.RequiredLength = 6;
			options.Password.RequiredUniqueChars = 1;

			// Lockout settings.
			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
			options.Lockout.MaxFailedAccessAttempts = 5;
			options.Lockout.AllowedForNewUsers = true;

			// User settings.
			options.User.AllowedUserNameCharacters =
				"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
			options.User.RequireUniqueEmail = false;
		});

		services.ConfigureApplicationCookie(options =>
		{
			// Cookie settings
			options.Cookie.HttpOnly = true;
			options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

			options.LoginPath = "/Identity/Account/Login";
			options.AccessDeniedPath = "/Identity/Account/AccessDenied";
			options.SlidingExpiration = true;
		});

		return services;
	}
}
