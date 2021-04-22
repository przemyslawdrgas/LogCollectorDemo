using LogCollectorDemo.Core.Entities;
using LogCollectorDemo.Core.Repositories;
using LogCollectorDemo.Core.Services;
using LogCollectorDemo.Data.Repositories;
using LogCollectorDemo.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace LogCollectorDemo
{

    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json", false)
            .Build();

            var serviceCollection = new ServiceCollection()
                .AddLogging(configure =>
                {
                    configure
                    .AddSimpleConsole(options =>
                    {
                        options.IncludeScopes = false;
                        options.SingleLine = true;
                        options.TimestampFormat = "HH:mm:ss ";
                    })
                    .SetMinimumLevel(LogLevel.Debug);
                })
                .AddTransient<ILogService, LogService>()
                .AddTransient<IEventService, EventService>()
                .AddTransient<ILogParser, LogParser>()
                .AddTransient<App>()
                .AddSingleton<IConfiguration>(configuration)
                .AddTransient<IRepository<EventEntity>, LiteDBRepository<EventEntity>>(x => new LiteDBRepository<EventEntity>(configuration.GetConnectionString("LiteDbDefault")))
                .BuildServiceProvider();

            serviceCollection.GetService<App>().Run();
        }
    }
}
