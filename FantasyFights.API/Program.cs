using System.Text.Json;
using FantasyFights.API.Middlewares;
using FantasyFights.BLL.Exceptions;
using FantasyFights.BLL.Services.AuthenticationService;
using FantasyFights.BLL.Services.CharactersService;
using FantasyFights.BLL.Services.UserRegistrationService;
using FantasyFights.DAL;
using FantasyFights.DAL.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Diagnostics;
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

app.UseExceptionHandler(applicationBuilder => applicationBuilder.Run(async httpContext =>
{
    var error = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
    if (error is HttpResponseException httpResponseException)
    {
        httpContext.Response.StatusCode = (int)httpResponseException.StatusCode;
        var responseBody = JsonSerializer.Serialize(new { error.Message });
        httpContext.Response.ContentType = "application/json; charset=UTF-8";
        await httpContext.Response.WriteAsync(responseBody);
    }
    else
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        var responseBody = JsonSerializer.Serialize(new { message = "Something went wrong." });
        httpContext.Response.ContentType = "application/json; charset=UTF-8";
        await httpContext.Response.WriteAsync(responseBody);
    }
}));

app.UseHttpsRedirection();

app.UseRegistrationDataCheck();

app.UseEmailConfirmationDataCheck();

app.UseEmailConfirmationCodeRequestDataCheck();

app.UseAuthorization();

app.MapControllers();

app.Run();
