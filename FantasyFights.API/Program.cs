using System.Text.Json;
using FantasyFights.API.Middlewares;
using FantasyFights.BLL.Exceptions;
using FantasyFights.BLL.Services.AuthenticationService;
using FantasyFights.BLL.Services.CharactersService;
using FantasyFights.BLL.Services.UserRegistrationService;
using FantasyFights.BLL.Utilities;
using FantasyFights.BLL.Utilities.TokenUtility;
using FantasyFights.DAL;
using FantasyFights.DAL.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configuration for SQL Server.
// builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDefaultConnection")));

// Configuration for MySQL Server.
builder.Services.AddDbContext<DatabaseContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("MySqlServerDefaultConnection"), new MySqlServerVersion(new Version(8, 0, 32))));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICharactersService, CharactersService>();
builder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<ITokenUtility, TokenUtility>();

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

app.UseExceptionHandler(applicationBuilder => applicationBuilder.Run(async httpContext =>
{
    var error = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
    httpContext.Response.StatusCode = error is HttpResponseException httpResponseException ? (int)httpResponseException.StatusCode : StatusCodes.Status500InternalServerError;
    httpContext.Response.ContentType = "application/json; charset=UTF-8";
    var errorMessage = error is HttpResponseException exception ? exception.Message : "Something went wrong.";
    await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new { message = errorMessage }));
}));

app.UseHttpsRedirection();

app.UseRegistrationDataCheck();

app.UseEmailConfirmationDataCheck();

app.UseEmailConfirmationCodeRequestDataCheck();

app.UseAuthorization();

app.MapControllers();

app.Run();
