﻿using AutoMapper;
using LibraryApp.Application.Common.Enums;
using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.Common.Models;
using LibraryApp.Application.User.Commands;
using LibraryApp.Domain;
using LibraryApp.Infrastructure.Identity.Models;
using LibraryApp.Infrastructure.Identity.Models.Authentication;
using LibraryApp.Infrastructure.Identity.Models.ChangeRole;
using LibraryApp.Infrastructure.Persistance;
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
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JWT _jwt;
        private readonly IMapper _mapper;

        public IdentityService(UserManager<AppUser> userManager, IOptions<JWT> jwt, IMapper mapper)
        {
            _userManager = userManager;
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
                var jwtSecurityToken = await CreateJwtTokenAsync(user);

                return new AuthentificationResponse(
                    message: string.Empty,
                    isAuthorized: true,
                    token: new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    roles: (await _userManager.GetRolesAsync(user).ConfigureAwait(false)).ToList());
            }

            return AuthentificationResponse.Denied($"Incorrect password for username {request.UserName}");
        }

        private async Task<JwtSecurityToken> CreateJwtTokenAsync(AppUser user)
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

        public async Task<RequestResult> RegisterAsync(UserRegistrationCommand data)
        {
            var username = await _userManager.FindByNameAsync(data.UserName);

            if (username != null)
                return RequestResult.Fail(RequestError.AlreadyExists, $"User with username {data.UserName} already registered");

            var user = _mapper.Map<UserRegistrationCommand, AppUser>(data);
            var result = await _userManager.CreateAsync(user, data.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Reader.ToString());
                return RequestResult.Success();
            }

            else return RequestResult.Fail(RequestError.ValidationError, result.Errors.Select(e => e.Description));
        }

        public async Task<RequestResult> ChangeRoleAsync(ChangeRoleRequest request, RoleActions action)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
                return RequestResult.Fail(RequestError.NotFound, $"No account registred with username {request.UserName}");

            // Checking if requested role exists
            var role = Enum.GetNames(typeof(Roles)).FirstOrDefault(r => string.Equals(r, request.Role, StringComparison.OrdinalIgnoreCase));

            if (role == null)
                return RequestResult.Fail(RequestError.NotFound, $"Role {request.UserName} not found");

            switch (action)
            {
                case RoleActions.Add:
                    if (await _userManager.IsInRoleAsync(user, role))
                        return RequestResult.Fail(RequestError.AlreadyExists, $"User {request.UserName} already has role {request.Role}");

                    await _userManager.AddToRoleAsync(user, role);
                    break;

                case RoleActions.Remove:
                    if (!await _userManager.IsInRoleAsync(user, role))
                        return RequestResult.Fail(RequestError.NotFound, $"User {request.UserName} currently has no role {request.Role}");

                    await _userManager.RemoveFromRoleAsync(user, role);
                    break;
            }

            return RequestResult.Success();
        }
    }
}
