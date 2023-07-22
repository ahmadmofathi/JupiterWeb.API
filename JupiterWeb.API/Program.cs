using JupiterWeb.API.Data;
using JupiterWeb.API.Data.Models;
using JupiterWeb.BL;
using JupiterWeb.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

//context registration 
var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register our Repo
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserManager, UserManager>();

// Register of UsersContext
builder.Services.AddDbContext<UsersContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("ConnectionString")));

// Authenticaction
var secretKey = builder.Configuration.GetValue<string>("SecretKey");
var secretKeyBytes = string.IsNullOrEmpty(secretKey) ? null : Encoding.ASCII.GetBytes(secretKey);
var Key = new SymmetricSecurityKey(secretKeyBytes);

builder.Services.AddAuthentication("default").AddJwtBearer("default", 
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            IssuerSigningKey = Key
        };
    }
    );
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
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
