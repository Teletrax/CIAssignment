using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FourC.Worker.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FourC.Worker.Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
                .Build();

            var serviceProvider = ConfigureServiceProvider(configuration);

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug)
                .CreateLogger<Program>();

            var source = CreateCancellationToken();

            try
            {
                MainAsync(serviceProvider, logger, source.Token).GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Cancelled");
            }
        }

        private static CancellationTokenSource CreateCancellationToken()
        {
            var source = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                source.Cancel();
            };
            return source;
        }

        private static IServiceProvider ConfigureServiceProvider(IConfigurationRoot configuration)
        {
            var connectionString = configuration["Data:ConnectionString"];
            var queueName = configuration["Queue:Name"];

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddFormatter()
                .AddQueue(queueName)
                .AddTransient<IDbService, DbService>(s => new DbService(connectionString))
                .AddTransient<IQueueService, QueueService>()
                .BuildServiceProvider();
            return serviceProvider;
        }

        private static async Task MainAsync(IServiceProvider serviceProvider, ILogger logger, CancellationToken cancellationToken)
        {
            logger.LogInformation("Waiting for messages...");

            var dbService = serviceProvider.GetRequiredService<IDbService>();
            var queueService = serviceProvider.GetRequiredService<IQueueService>();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var model = await queueService.ReceiveAsync(cancellationToken);
                        logger.LogInformation("Message received: {0}", model);

                        await dbService.SaveWorkAsync(model, cancellationToken);
                        logger.LogInformation("Saved to DB: {0}", model);

                        scope.Complete();
                    }
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    logger.LogError(new EventId(0), ex, "Error processing message");

                    logger.LogInformation("Waiting for 5 secs");
                    await Task.Delay(5000, cancellationToken);
                }
            }
        }
    }
}
