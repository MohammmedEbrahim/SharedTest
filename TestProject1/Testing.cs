using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SharedZone.Server;


namespace TestProject1
{


    [SetUpFixture]
    public class Testing
    {
        private static IConfigurationRoot         _configuration;
        private static IWebHostEnvironment        _env;
        private static IServiceScopeFactory       _scopeFactory;
        private static IServiceCollection         _services;
        private static Checkpoint                 _checkpoint;


        [OneTimeSetUp]
        [Obsolete]
        public void RunBeforeAnyTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            _services = new ServiceCollection();

            _env = Mock.Of<IWebHostEnvironment>(w =>
                w.EnvironmentName == "Development" &&
                w.ApplicationName == "SharedZone.API");

            var startup = new Startup(_configuration, _env);

            _services.AddLogging();

            startup.ConfigureServices(_services);

            _scopeFactory = _services.BuildServiceProvider().GetService<IServiceScopeFactory>();

            
            EnsureDatabase();
        }

        private static void EnsureDatabase()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            if (context is null) throw new ArgumentNullException(nameof(context));

            context.Database.Migrate();
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            if (mediator is null) throw new ArgumentNullException(nameof(mediator));

            return await mediator.Send(request);
        }

        public static async Task SendAsync(IRequest request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            if (mediator is null) throw new ArgumentNullException(nameof(mediator));

            await mediator.Send(request);
        }

        public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            if (context is null) throw new ArgumentNullException(nameof(context));

            return await context.FindAsync<TEntity>(keyValues);
        }

        public static async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            if (context is null) throw new ArgumentNullException(nameof(context));

            context.Add(entity);

            await context.SaveChangesAsync();
        }

        public static async Task<int> CountAsync<TEntity>() where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            if (context is null) throw new ArgumentNullException(nameof(context));

            return await context.Set<TEntity>().CountAsync();
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            if (context is null) throw new ArgumentNullException(nameof(context));

            context.Database.EnsureDeleted();
        }

        public static IFormFile MockFile(string extension)
        {
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(_ => _.ContentType).Returns(extension);

            return fileMock.Object;
        }
    }
}
