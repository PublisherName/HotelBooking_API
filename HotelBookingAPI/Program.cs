using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BookingAPI.Data;
using GuestAPI.Service;
using BookingAPI.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                        .AllowAnyHeader();
        });
});

builder.Services.AddSingleton(x =>
{
    var secretKeyString = builder.Configuration.GetSection("AppSettings")["SecretKey"] ?? throw new ArgumentException("Secret key is required");
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

app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
