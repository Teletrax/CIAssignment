using System.Messaging;
using FourC.Worker.Core;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace FourC.Worker.Backend
{
    public static class FormatterExtensions
    {
        public static IServiceCollection AddFormatter(this IServiceCollection collection)
        {
            var formatter = new JsonMessageFormatter(new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
            return collection.AddSingleton<IMessageFormatter>(formatter);
        }
    }
}