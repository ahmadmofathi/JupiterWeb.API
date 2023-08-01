using JupiterWeb.API.Data;
using JupiterWeb.BL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using JupiterWeb.DAL.Repos.Users;
using JupiterWeb.DAL;

var builder = WebApplication.CreateBuilder(args);

//context registration 
builder.Services.AddIdentity<User, IdentityRole<string>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); 
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
// Register our Repo
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<ITaskRepo, TasksRepo>();
builder.Services.AddScoped<ITaskManager, TasksManager>();
builder.Services.AddScoped<IRequestRepo, RequestRepo>();

builder.Services.AddAuthorization();
// Injection of UserManager 


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
