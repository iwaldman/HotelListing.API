using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.API.Repositories;

public class AuthManager : IAuthManager
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthManager(
        IMapper mapper,
        UserManager<ApiUser> userManager,
        IConfiguration configuration
    )
    {
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto?> Login(LoginDto loginDto)
    {
        var isValidUser = false;

        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return null;
        }

        isValidUser = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (isValidUser == false)
        {
            return null;
        }

        var token = await GenerateToken(user);

        return new AuthResponseDto { UserId = user.Id, Token = token };
    }

    public async Task<IEnumerable<IdentityError>> Register(ApiUserDto apiUserDto)
    {
        var user = _mapper.Map<ApiUser>(apiUserDto);
        user.UserName = apiUserDto.Email;

        var result = await _userManager.CreateAsync(user, apiUserDto.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "user");
        }

        return result.Errors;
    }

    private async Task<string> GenerateToken(ApiUser apiUser)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(apiUser);

        var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();

        var userClaims = await _userManager.GetClaimsAsync(apiUser);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, apiUser.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, apiUser.Email!),
            new Claim("uid", apiUser.Id)
        }
            .Union(roleClaims)
            .Union(userClaims);

        var strVal = _configuration["Jwt:DurationInMinutes"];
        var durationInMinutes = int.TryParse(strVal, out int intVal) ? intVal : 10;

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(durationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
