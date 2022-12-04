using Microsoft.EntityFrameworkCore;

using MagicVillaAPI.DBContext;
using MagicVillaAPI.Logging;

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

// Adding Database
builder.Services.AddDbContext<VillaDBContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
    Console.WriteLine("In Program.cs");
});

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
