using FakeUser.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FakeUser.Infrastructure.Context;
using FakeUser.Service.Map;
using Hangfire;
using FakeUser.Core.Configurations;
using FakeUser.Core.Caching;
using FakeUser.Service.Services;
using FakeUser.Core.Interfaces;
using FakeUser.Infrastructure.Repositories;

namespace FakeUser.BackgroundServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddDbContext<FakeUserDbContext>(options =>
                    options.UseSqlServer(
                        hostContext.Configuration.GetConnectionString("DefaultConnection")));

                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddScoped<IFakeUserRepository, FakeUserRepository>();
                    services.AddScoped<IFakeUserService, FakeUserService>();
                    services.AddAutoMapper(typeof(MappingProfile));
                    services.AddHangfireServer();
                    services.AddHangfire(x => x.UseSqlServerStorage(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                    services.Configure<FakeUserCacheConfiguration>(hostContext.Configuration.GetSection("CacheConfiguration"));

                    services.AddMemoryCache();
                    services.AddTransient<ICacheService, MemoryCacheService>();
                });
    }
}
