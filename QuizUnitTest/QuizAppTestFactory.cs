using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;


namespace QuizUnitTest
{
    public class QuizAppTestFactory<TProgram>
       : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<QuizDbContext>));
                services.Remove(dbContextDescriptor);
                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbConnection));

                services.Remove(dbConnectionDescriptor);

                services
                    .AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<QuizDbContext>((container, options) =>
                    {
                        options.UseInMemoryDatabase("QuizUnitTest").UseInternalServiceProvider(container);
                    });
            });
            builder.UseEnvironment("Development");
        }
    }
}
