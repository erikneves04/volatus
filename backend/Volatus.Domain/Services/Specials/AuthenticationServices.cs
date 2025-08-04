using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Volatus.Domain.Interfaces.Services.Specials;
using Volatus.Domain.View;
using Volatus.Domain.Entities;
using Volatus.Domain.Interfaces.Repositories;
using Volatus.Domain.Exceptions;

namespace Volatus.Domain.Services.Specials;

public class AuthenticationServices : IAuthenticationServices
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;
    private readonly IPasswordServices _passwordServices;

    public AuthenticationServices(IUserRepository userRepository, IConfiguration config, IPasswordServices passwordServices)
    {
        _userRepository = userRepository;
        _config = config;
        _passwordServices = passwordServices;
    }

    public UserTokenViewModel Login(LoginParams login)
    {
        ThrowIfLoginIsInvalid(login);

        var user = GetUser(login.Email);

        if (user is null)
            throw new BadRequestException("Invalid credentials.");

        if (!_passwordServices.Check(login.Password, user.Password))
            throw new BadRequestException("Invalid credentials.");

        return GenerateToken(user);
    }

    private User GetUser(string email)
    {
        return _userRepository
                .Query()
                .Include(e => e.Permissions)
                .ThenInclude(e => e.Permission)
                .Where(e => e.Email == email)
                .FirstOrDefault();
    }

    private UserTokenViewModel GenerateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim("userId", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expirationConfig = _config["TokenConfiguration:ExpireHours"];
        var expiration = DateTime.UtcNow.AddHours(double.Parse(expirationConfig));

        var token = new JwtSecurityToken(
            issuer: _config["TokenConfiguration:Issuer"],
            audience: _config["TokenConfiguration:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credenciais);

        return new UserTokenViewModel()
        {
            UserId = user.Id,
            Permissions = user.Permissions.Select(e => e.Permission.Name).ToList(),
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiration,
        };
    }

    private static void ThrowIfLoginIsInvalid(LoginParams login)
    {
        if (login is null)
            throw new BadRequestException("Model is required.");

        var messages = new List<string>();

        if (string.IsNullOrEmpty(login.Email))
            messages.Add("Email is required.");

        if (string.IsNullOrEmpty(login.Password))
            messages.Add("Password is required.");

        if (messages.Any())
            throw new BadRequestException(messages);
    }
}