using Microsoft.EntityFrameworkCore;

using MagicVillaAPI.DBContext;
using MagicVillaAPI.Logging;
using MagicVillaAPI;
using MagicVillaAPI.Repository.IRepository;
using MagicVillaAPI.Repository.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
.AddControllers(opt =>
{
    //opt.ReturnHttpNotAcceptable=true;
})
.AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


// Adding Custom logger to container for dependency injection
builder.Services.AddSingleton<ILoggingCustom, LoggingCustomImpl>();
builder.Services.AddScoped<IVillaRepository, VillaRepositoryImpl>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepositoryImpl>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
var key = builder.Configuration.GetValue<string>("ApiSettings:Secret") ?? "";
builder.Services.AddAuthentication( x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer( x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
	{
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Adding Database
builder.Services.AddDbContext<ApplicationDBContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
    opt.EnableSensitiveDataLogging();
});
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});
builder.Services.AddSwaggerGen(opt =>
{
	opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "Type: Bearer [space] Token",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Scheme = "Bearer"
	});
	opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				},
				Scheme = "oauth2",
				Name = "Bearer",
				In = ParameterLocation.Header
			},
			new List<string>()
		}
	});
	opt.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1.0",
		Title = "Magic Villa",
		Description = "API to manage Villa",
		TermsOfService = new Uri("https://www.google.com"),
		Contact = new OpenApiContact
		{
			Name = "A N M Jubaer",
			Url = new Uri("https://github.com/jubaer-ad"),
			Email = "jubaerad1@gmail.com"
		},
		License = new OpenApiLicense
		{
			Name = "License",
			Url = new Uri("https://www.google.com")
		}
	});
	opt.SwaggerDoc("v2", new OpenApiInfo
	{
		Version = "v2.0",
		Title = "Magic Villa",
		Description = "API to manage Villa",
		TermsOfService = new Uri("https://www.google.com"),
		Contact = new OpenApiContact
		{
			Name = "A N M Jubaer",
			Url = new Uri("https://github.com/jubaer-ad"),
			Email = "jubaerad1@gmail.com"
		},
		License = new OpenApiLicense
		{
			Name = "License",
			Url = new Uri("https://www.google.com")
		}
	});
});

//builder.Services.AddDbContext<VillaDBContext>(opt => opt.UseInMemoryDatabase("VillaDB"));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("v1/swagger.json", "Magic_Villa_v1");
		opt.SwaggerEndpoint("v2/swagger.json", "Magic_Villa_v2");
	});
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
