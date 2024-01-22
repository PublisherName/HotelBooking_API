using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BookingAPI.Data;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(x =>
{
    var secretKeyString = builder.Configuration["SECRET_KEY"] ?? throw new ArgumentException("Secret key is required");
    return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyString));
});

builder.Services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase(builder.Configuration["DB_NAME"] ?? throw new ArgumentException("DB_NAME is required")));

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
