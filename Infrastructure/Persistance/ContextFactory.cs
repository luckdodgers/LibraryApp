using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;

namespace LibraryApp.Infrastructure.Persistance
{
    public class ContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            System.Diagnostics.Debug.WriteLine($"Testing appsettings path: {Path.Combine(AppDomain.CurrentDomain.BaseDirectory)}");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
