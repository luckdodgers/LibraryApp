using AutoMapper;
using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.Common.Models;
using LibraryApp.Application.User.Commands;
using LibraryApp.Domain;
using LibraryApp.Domain.Entities;
using LibraryApp.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _mapper = mapper;
        }

        public async Task<AuthentificationResponse> GetTokenAsync(TokenRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
                return AuthentificationResponse.Denied($"No account with username {request.UserName} found");

            if (await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var jwtSecurityToken = await CreateJwtToken(user);

                return new AuthentificationResponse(
                    message: string.Empty,
                    isAuthorized: true,
                    token: new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    roles: (await _userManager.GetRolesAsync(user).ConfigureAwait(false)).ToList());
            }

            return AuthentificationResponse.Denied($"Incorrect password for username {request.UserName}");
        }

        private async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<Result> RegisterAsync(UserRegistrationCommand data)
        {
            var username = await _userManager.FindByNameAsync(data.UserName);

            if (username != null)
                return Result.Fail(new string[] { $"User with username {data.UserName} already registered" });

            var user = _mapper.Map<UserRegistrationCommand, AppUser>(data);
            var result = await _userManager.CreateAsync(user, data.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Reader.ToString());
                return Result.Success();
            }

            else return Result.Fail(result.Errors.Select(e => e.Description));
        }
    }
}
