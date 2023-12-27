using BankingBL.Config;
using BankingBL.Services;
using BankingDAL;
using BankingDAL.Entities;
using BankingDAL.Repositories;
using BankingDAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Services.AddDbContext<BankingDBContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

// Use in-memory database for testing
builder.Services.AddDbContext<BankingDBContext>(o =>{o.UseInMemoryDatabase("InMemoryBankingDatabase");});

builder.Services.Configure<BankingApiConfig>(builder.Configuration.GetSection("BankingApiConfig"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountServices, AccountService>();

builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<RequestLoggingMiddleware>();
app.MapControllers();

app.Run();
