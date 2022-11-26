using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using FakeUser.Infrastructure.Context;
using FakeUser.Core.Caching;
using FakeUser.Infrastructure.Interfaces;
using FakeUser.Core.Configurations;
using FakeUser.Service.Services;
using FakeUser.Core.Interfaces;
using FakeUser.Infrastructure.Repositories;
using FakeUser.Service.Map;

namespace FakeUserAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddHangfire(x => x.UseSqlServerStorage("DefaultConnection"));
            services.AddHangfireServer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FakeUserAPI", Version = "v1" });
            });
            services.AddDbContext<FakeUserDbContext>
               (opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IFakeUserRepository, FakeUserRepository>();
            services.AddScoped<IFakeUserService, FakeUserService>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.Configure<FakeUserCacheConfiguration>(Configuration.GetSection("CacheConfiguration"));
            services.AddMemoryCache();
            services.AddTransient<ICacheService, MemoryCacheService>();
            services.AddHangfireServer();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FakeUserAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
