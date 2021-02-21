using LibraryApp.Application.Domain.Entities;
using LibraryApp.Domain.Entities;
using LibraryApp.Infrastructure.Identity.Models;
using LibraryApp.Infrastructure.Persistance;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryApp.Tests.Application.IntegrationTests
{
    using static TestSetup;

    class ScopedRequest
    {
        public static int UserCardId { get; private set; }

        private const string userName = "testUsername";
        private const string password = "Password1234!";

        public static async Task<string> RunAsDefaultUserAsync() => await RunAsUserAsync(userName, password);

        public static async Task<string> RunAsUserAsync(string userName, string password)
        {
            var scope = ScopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();

            var user = new AppUser { UserName = userName };
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                //_currentUserId = user.Id;
                var context = scope.ServiceProvider.GetService<AppDbContext>();
                await context.Cards.AddAsync(new Card(userName));
                await context.SaveChangesAsync();

                var cardFromDb = await context.Cards.FirstOrDefaultAsync(c => c.UserName == userName);
                UserCardId = cardFromDb.Id;
            }

            return user.UserName;
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            var scope = ScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();

            return await mediator.Send(request);
        }

        public static async Task AddAsync<TEntity>(TEntity entity) 
            where TEntity : IDomainEntity
        {
            var scope = ScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            context.Add(entity);
        
            await context.SaveChangesAsync();
        }

        public static async Task<Book> GetBookAsync(string title)
        {
            var scope = ScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            return await context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Title == title);
        }

        public static async Task<Card> GetDefaultUserCardAsync()
        {
            var scope = ScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            return await context.Cards
                .Include(c => c.Books)
                .FirstAsync(c => c.Id == UserCardId);
        }

        public static async Task AddBookToCardAsync(Book book, string username = userName)
        {
            var scope = ScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            var card = await context.Cards.Include(c => c.Books).FirstAsync(c => c.UserName == userName);

            card.TryAddBook(book);

            await context.SaveChangesAsync();
        }

        public static void ResetState()
        {
            UserCardId = -1;
        }
    }
}
