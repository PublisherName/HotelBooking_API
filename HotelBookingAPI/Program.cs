using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BookingAPI.Data;
using GuestAPI.Service;
using BookingAPI.Service;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(x =>
{
    var secretKeyString = builder.Configuration["SECRET_KEY"] ?? throw new ArgumentException("Secret key is required");
    return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyString));
});

builder.Services.AddDbContext<ApiContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(7, 0, 0))
    )
);

builder.Services.AddScoped<GuestService>();
builder.Services.AddScoped<BookingService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
