using Microsoft.EntityFrameworkCore;

using MagicVillaAPI.DBContext;
using MagicVillaAPI.Logging;
using MagicVillaAPI;
using MagicVillaAPI.Repository.IRepository;
using MagicVillaAPI.Repository.Implementation;

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
builder.Services.AddSwaggerGen();

// Adding Custom logger to container for dependency injection
builder.Services.AddSingleton<ILoggingCustom, LoggingCustomImpl>();
builder.Services.AddScoped<IVillaRepository, VillaRepositoryImpl>();

// Adding Database
builder.Services.AddDbContext<ApplicationDBContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});
builder.Services.AddAutoMapper(typeof(MappingConfig));

//builder.Services.AddDbContext<VillaDBContext>(opt => opt.UseInMemoryDatabase("VillaDB"));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
