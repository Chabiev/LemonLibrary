using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Business.DTOs;
using Database.Data;
using Database.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Lemon_Library.Controllers;

public class UserController : BaseApiController
{
    private readonly LibraryContext _context;

    public UserController(LibraryContext context)
    {
        _context = context;
    }
    
    
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserDTO userDto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);

        if (existingUser != null)
        {
            // Username already exists, return a bad request response indicating the duplicate username
            return BadRequest("Username already exists.");
        }

        CreatePasswordHash(userDto.Password, out byte[] passwordHash,out byte[] passwordSalt );

        var result = new User
        {
            Username = userDto.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
         _context.Users.Add(result);
         await _context.SaveChangesAsync();

         return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDTO userDto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == userDto.Username);

        if (user == null)
        {
            return BadRequest("User not found");
        }

        if (!VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest("Wrong Password.");
        }

        string token = CreateToken(user);

        return Ok(token);
    }

    private string CreateToken(User user)
    {
        
        return string.Empty;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
    
}