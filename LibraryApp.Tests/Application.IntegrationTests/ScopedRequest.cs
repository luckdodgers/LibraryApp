using LibraryApp.Application.Infrastructure.Identity;
using LibraryApp.Infrastructure.Identity.Models;
using LibraryApp.Infrastructure.Persistance;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibraryApp.Domain.Entities;


namespace LibraryApp.Tests.Application.IntegrationTests
{
    using static TestSetup;

    class ScopedRequest
    {
        public static async Task<string> RunAsDefaultUserAsync()
        {
            const string userName = "testUsername";
            const string password = "Password1234!";

            return await RunAsUserAsync(userName, password);
        }

        public static async Task<string> RunAsUserAsync(string userName, string password)
        {
            using var scope = ScopeFactory.CreateScope();

            var userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();
            var user = new AppUser { UserName = userName };
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                //_currentUserId = user.Id;
                var context = scope.ServiceProvider.GetService<AppDbContext>();
                await context.Cards.AddAsync(new Card(userName));
                return user.UserName;
            }

            var errors = string.Join(Environment.NewLine, result.ToApplicationResult().Errors);

            throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = ScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();

            return await mediator.Send(request);
        }

        public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
        {
            using var scope = ScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            return await context.FindAsync<TEntity>(keyValues);
        }
    }
}
