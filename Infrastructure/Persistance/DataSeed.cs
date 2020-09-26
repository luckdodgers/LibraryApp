using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.User.Commands;
using LibraryApp.Domain;
using LibraryApp.Domain.Entities;
using LibraryApp.Infrastructure.Identity;
using LibraryApp.Infrastructure.Identity.Models;
using LibraryApp.Infrastructure.Identity.Models.AddRole;
using LibraryApp.Infrastructure.Identity.Models.ChangeRole;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Persistance
{
    public static class DataSeed
    {
        public static async Task EnsureSeedData(IServiceProvider provider)
        {
            var dbContext = provider.GetRequiredService<AppDbContext>();
            var userManager = provider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

            await dbContext.Database.MigrateAsync();

            await SeedBooksAsync(dbContext);
            await SeedEssentialsAsync(userManager, roleManager);
        }

        private static async Task SeedBooksAsync(IApplicationDbContext context)
        {
            // Don't seed if any book already exists
            if (await context.Books.AnyAsync())
                return;

            var book_1 = new Book("Introduction to Quantum Mechanics"); // Authors: David J. Griffiths, Darrell F. Schroeter
            var book_2 = new Book("Introduction to Electrodynamics"); // Author: David J. Griffiths

            var books_authors = new List<Author>() { new Author("David J. Griffiths"), new Author("Darrell F. Schroeter") };

            await context.Books.AddAsync(book_1);
            await context.Books.AddAsync(book_2);
            await context.Authors.AddRangeAsync(books_authors);

            await context.SaveChangesAsync();

            var bookAuthors = new List<BookAuthor>() 
            { 
                new BookAuthor(books_authors[0].Id, books_authors[0], book_1.Id, book_1),
                new BookAuthor(books_authors[0].Id, books_authors[0], book_2.Id, book_2),
                new BookAuthor(books_authors[1].Id, books_authors[1], book_1.Id, book_1),
            };

            context.BookAuthors.AddRange(bookAuthors);

            book_1.SetAuthors(bookAuthors);
            book_2.SetAuthors(bookAuthors[0]);

            books_authors[0].AddBook(bookAuthors[0]);
            books_authors[0].AddBook(bookAuthors[1]);
            books_authors[1].AddBook(bookAuthors[2]);

            await context.SaveChangesAsync();
        }

        public static async Task SeedEssentialsAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Don't seed if admin role already exists
            if (await roleManager.RoleExistsAsync(Roles.Admin.ToString()))
                return;

            // Seeding roles
            foreach (var role in Enum.GetNames(typeof(Roles)))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Seeding default User for testing purpose
            var defaultUser = new DefaultUser();

            if (!userManager.Users.Any(u => u.UserName == defaultUser.UserName))
            {
                await userManager.CreateAsync(defaultUser, DefaultUser.default_password);
                await userManager.AddToRoleAsync(defaultUser, DefaultUser.default_role.ToString());
            }

            // Seeding admin account
            var defaultAdmin = new AppUser()
            {
                UserName = "DefaultAdmin",
                FirstName = "Default",
                LastName = "Admin"
            };

            if (!userManager.Users.Any(u => u.UserName == defaultAdmin.UserName))
            {
                await userManager.CreateAsync(defaultAdmin, "G1$5fmnsIa8");
                await userManager.AddToRoleAsync(defaultAdmin, Roles.Admin.ToString());
            }
        }
    }
}
