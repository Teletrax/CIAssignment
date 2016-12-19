using Microsoft.Extensions.DependencyInjection;

namespace FourC.Worker.Core
{
    public static class QueueExtensions
    {
        public static IServiceCollection AddQueue(this IServiceCollection collection,string queueName)
        {
            return collection.AddSingleton(QueueFactory.CreateQueue(queueName));
        }
    }
}