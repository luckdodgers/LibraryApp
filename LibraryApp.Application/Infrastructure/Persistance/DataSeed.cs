using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.User.Commands;
using LibraryApp.Domain;
using LibraryApp.Domain.Entities;
using LibraryApp.Infrastructure.Identity.Models.ChangeRole;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Persistance
{
    public static class DataSeed
    {
        public static async Task EnsureSeedData(IServiceProvider provider)
        {
            var dbContext = provider.GetRequiredService<AppDbContext>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var mediator = provider.GetRequiredService<IMediator>();
            var userService = provider.GetRequiredService<IIdentityService>();

            await dbContext.Database.MigrateAsync();

            await SeedBooksAsync(dbContext);
            await SeedEssentialsAsync(mediator, userService, roleManager);
        }

        private static async Task SeedBooksAsync(IApplicationDbContext context)
        {
            // Don't seed if any book already exists
            if (await context.Books.AnyAsync())
                return;

            var book_1 = new Book("Introduction to Quantum Mechanics"); // Authors: David J. Griffiths, Darrell F. Schroeter
            var book_2 = new Book("Introduction to Electrodynamics"); // Author: David J. Griffiths

            var authors = new List<Author>() { new Author("David J. Griffiths"), new Author("Darrell F. Schroeter") };

            book_1.SetAuthors(authors);
            book_2.SetAuthor(authors[0]);

            await context.Books.AddAsync(book_1);
            await context.Books.AddAsync(book_2);

            await context.SaveChangesAsync();
        }

        public static async Task SeedEssentialsAsync(IMediator mediator, IIdentityService userService, RoleManager<IdentityRole> roleManager)
        {
            // Seeding roles
            foreach (var role in Enum.GetNames(typeof(Roles)))
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var defaultAdminRegCmd = new UserRegistrationCommand()
            {
                FirstName = "Default",
                LastName = "Admin",
                Password = "G1$5fmnsIa8",
                UserName = "DefaultAdmin"
            };

            // Seeding default admin
            await mediator.Send(defaultAdminRegCmd);

            var changeRoleRequest = new ChangeRoleRequest()
            {
                Role = Roles.Admin.ToString(),
                UserName = defaultAdminRegCmd.UserName
            };

            // Adding Admin role
            await userService.ChangeRoleAsync(changeRoleRequest, RoleActions.Add);

            var defaultReaderRegCmd = new UserRegistrationCommand()
            {
                FirstName = "Default",
                LastName = "Reader",
                Password = "9A!m*8Kn",
                UserName = "DefaultUser"
            };

            // Seeding default user
            await mediator.Send(defaultReaderRegCmd);
        }
    }
}
