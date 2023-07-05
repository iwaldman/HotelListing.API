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
    private const string LoginProvider = "HotelListingApi";
    private const string RefreshToken = "RefreshToken";
    private const string UserRole = "user";

    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;
    private readonly IConfiguration _configuration;
    private ApiUser? _user;

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

    public async Task<string> CreateRefreshToken()
    {
        await _userManager.RemoveAuthenticationTokenAsync(_user!, LoginProvider, RefreshToken);

        var newRefreshToken = await _userManager.GenerateUserTokenAsync(
            _user!,
            LoginProvider,
            RefreshToken
        );

        _ = await _userManager.SetAuthenticationTokenAsync(
            _user!,
            LoginProvider,
            RefreshToken,
            newRefreshToken
        );

        return newRefreshToken;
    }

    public async Task<AuthResponseDto?> Login(LoginDto loginDto)
    {
        var _user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (_user is null)
        {
            return null;
        }

        var isValidUser = await _userManager.CheckPasswordAsync(_user, loginDto.Password);
        if (isValidUser is false)
        {
            return null;
        }

        var token = await GenerateToken();

        return new AuthResponseDto
        {
            UserId = _user.Id,
            Token = token,
            RefreshToken = await CreateRefreshToken()
        };
    }

    public async Task<IEnumerable<IdentityError>> Register(ApiUserDto apiUserDto)
    {
        _user = _mapper.Map<ApiUser>(apiUserDto);
        _user.UserName = apiUserDto.Email;

        var result = await _userManager.CreateAsync(_user, apiUserDto.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(_user, UserRole);
        }

        return result.Errors;
    }

    public async Task<AuthResponseDto?> VerifyRefreshToken(AuthResponseDto authResponseDto)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(authResponseDto.Token);
        var username = tokenContent.Claims
            .ToList()
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)
            ?.Value;

        _user = await _userManager.FindByEmailAsync(username!);

        if (_user is null || _user.Id != authResponseDto.UserId)
        {
            return null;
        }

        var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(
            _user,
            LoginProvider,
            RefreshToken,
            authResponseDto.Token!
        );

        if (isValidRefreshToken)
        {
            var token = await GenerateToken();

            return new AuthResponseDto
            {
                UserId = _user.Id,
                Token = token,
                RefreshToken = await CreateRefreshToken()
            };
        }

        await _userManager.UpdateSecurityStampAsync(_user);

        return null;
    }

    private async Task<string> GenerateToken()
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(_user!);

        var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();

        var userClaims = await _userManager.GetClaimsAsync(_user!);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, _user!.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, _user!.Email!),
            new Claim("uid", _user.Id)
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
