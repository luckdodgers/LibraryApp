using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Respawn;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Moq;
using Microsoft.AspNetCore.Hosting;
using LibraryApp.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Tests.Application.IntegrationTests
{
    [SetUpFixture]
    class TestSetup
    {
        public static IServiceScopeFactory ScopeFactory { get; private set; }

        private static IConfigurationRoot _configuration;
        private static Checkpoint _checkpoint;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

            _configuration = builder.Build();

            var startup = new Startup(_configuration);

            var services = new ServiceCollection();

            services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
                w.EnvironmentName == "Development" &&
                w.ApplicationName == "LibraryApp.Application"));

            services.AddLogging();

            startup.ConfigureServices(services);

            ScopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new[] { "__EFMigrationsHistory" }
            };

            EnsureDatabase();
        }

        private static void EnsureDatabase()
        {
            using var scope = ScopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<AppDbContext>();

            context.Database.Migrate();
        }

        public static async Task ResetState()
        {
            ScopedRequest.ResetState();
            await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
