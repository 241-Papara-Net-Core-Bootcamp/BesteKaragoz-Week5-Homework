using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FakeUser.Infrastructure.Dtos;
using FakeUser.Infrastructure.Interfaces;

namespace FakeUser.BackgroundServices
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<BackgroundWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<BackgroundWorker> logger, IServiceScopeFactory scopeFactory)
        {
            logger = _logger;
            scopeFactory = _scopeFactory;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            {
                _logger.LogInformation("{Type} is now running in the background", nameof(BackgroundWorker));
                await BackgroundProcessing(stoppingToken);
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogCritical("The {Type} is stopping due to host shutdown, " +
                "queued item might not processed anymore", nameof(BackgroundWorker));
            return base.StopAsync(cancellationToken);
        }

        private async Task BackgroundProcessing(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine("Test");
                    await Task.Delay(5000, cancellationToken);
                    using var httpClient = new HttpClient();


                    var result = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");

                    var jsonString = await result.Content.ReadAsStringAsync();

                    var users = JsonSerializer.Deserialize<List<FakeUserDto>>(jsonString);

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var userService = scope.ServiceProvider.GetRequiredService<IFakeUserService>();
                        foreach (var item in users)
                        {
                            await userService.Add(item);
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogCritical("An error occured when publishing a book exception {Exception}", ex);
                }
            }
        }
    }
}
