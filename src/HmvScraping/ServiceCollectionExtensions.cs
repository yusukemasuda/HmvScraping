namespace HmvScraping
{
    using HmvScraping.Services;
    using HmvScraping.Utils;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Http;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;

    public static class ServiceCollectionExtensions
    {
        public static void AddHmvScraping(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddHttpClient();

            services.AddSingleton<IHttpUtils>((provider) => new HttpUtils(provider.GetRequiredService<IHttpClientFactory>()));
            services.AddSingleton<ISearchService>((provider) => new SearchService(provider.GetRequiredService<IHttpUtils>()));
            services.AddSingleton<IProductService>((provider) => new ProductService(provider.GetRequiredService<IHttpUtils>()));
            services.AddSingleton<IStockService>((provider) => new StockService(provider.GetRequiredService<IHttpUtils>()));
        }
    }
}
