using DocumentsExternal.Models.Interfaces;
using DocumentsExternal.Services;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Net.Http;

namespace DocumentsExternal.Extenssions
{
    public static class ServiceCollectionExtenssions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IAvaxApiClient, AvaxApiClient>()
                .AddTransientHttpErrorPolicy(policy => AddWithRetry(policy));

            return services;
        }

        private static AsyncPolicy<HttpResponseMessage> AddWithRetry(PolicyBuilder<HttpResponseMessage> policy)
        {
            return policy.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromMilliseconds(200),
                TimeSpan.FromMilliseconds(500),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5)
            });
        }
    }
}