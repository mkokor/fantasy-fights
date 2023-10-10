using FantasyFights.API.Middleware;
using FantasyFights.BLL.Services.AuthenticationService;
using FantasyFights.BLL.Services.CharactersService;
using FantasyFights.BLL.Services.UserRegistrationService;
using FantasyFights.DAL;
using FantasyFights.DAL.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICharactersService, CharactersService>();
builder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRegistrationDataCheck();

app.UseEmailConfirmationDataCheck();

app.UseEmailConfirmationCodeRequestDataCheck();

app.UseAuthorization();

app.MapControllers();

app.Run();
