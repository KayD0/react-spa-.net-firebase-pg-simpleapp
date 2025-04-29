using ProdBase.Infrastructure;
using ProdBase.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true);

// Add services to the container.
builder.Services.AddControllers();

// Add application configuration and services
builder.Services.AddApplicationConfiguration(builder.Configuration);
builder.Services.AddApplicationServices();

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
