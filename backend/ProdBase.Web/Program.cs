using Microsoft.EntityFrameworkCore;
using ProdBase.Application.Interfaces;
using ProdBase.Application.Services;
using ProdBase.Domain.Interfaces;
using ProdBase.Infrastructure.Repository;
using ProdBase.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true);

// Add services to the container.
builder.Services.AddControllers();

// Configure database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    $"Host={builder.Configuration["DB_HOST"] ?? "localhost"};" +
    $"Port={builder.Configuration["DB_PORT"] ?? "5432"};" +
    $"Database={builder.Configuration["DB_NAME"] ?? "youtubeapp"};" +
    $"Username={builder.Configuration["DB_USER"] ?? "postgres"};" +
    $"Password={builder.Configuration["DB_PASSWORD"] ?? "postgres"};";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register repositories
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();

// Register services
builder.Services.AddSingleton<IAuthService, FirebaseAuth>();
builder.Services.AddScoped<IProfileService, ProfileService>();

// Configure CORS
var corsOrigin = builder.Configuration["CORS_ORIGIN"] ?? "http://localhost:3000";
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(corsOrigin)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

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

// Use CORS
app.UseCors();

// Use Firebase Authentication
app.UseFirebaseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
