using Microsoft.AspNetCore.Mvc;
using LoginAPI.Models;
using BookingAPI.Data;
using GuestAPI.Models;
using Microsoft.EntityFrameworkCore;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace LoginAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController(ApiContext context, SymmetricSecurityKey secretKey) : ControllerBase
    {
        private readonly ApiContext _context = context;
        private readonly SymmetricSecurityKey _secretKey = secretKey;

        [HttpPost]
        public async Task<ActionResult> Login(Login logins)
        {
            var guestInDb = await _context.Guests
                .FirstOrDefaultAsync(g => g.GuestEmail == logins.Email);

            if (guestInDb == null)
            {
                return NotFound(new { message = "User not found" });
            }

            if (guestInDb.GuestPassword == null || logins.Password == null)
            {
                return Unauthorized(new { message = "Invalid password" });
            }

            if (VerifyPassword(logins.Password, guestInDb.GuestPassword))
            {
                return Ok(new
                {
                    Id = guestInDb.Id,
                    Email = guestInDb.GuestEmail,
                    Token = GenerateJwtToken(guestInDb)
                });
            }

            return Unauthorized(new { message = "Invalid password" });
        }

        private static bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return enteredPassword == storedPassword;
        }

        private string GenerateJwtToken(Guest guest)
        {
            var signinCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);

            if (guest.GuestEmail == null)
            {
                throw new ArgumentException("Guest email and id are required");
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, guest.GuestEmail),
                new(ClaimTypes.NameIdentifier, guest.Id.ToString())
            };

            var tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost",
                audience: "http://localhost",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }
    }
}

