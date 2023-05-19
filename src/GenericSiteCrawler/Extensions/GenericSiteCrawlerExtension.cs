
using GenericSiteCrawler.Core.Interfaces;
using GenericSiteCrawler.Crawlers;
using Microsoft.Extensions.DependencyInjection;
using System;  
namespace GenericSiteCrawler.Extensions;
public static class GenericSiteCrawlerExtension
{
    public static void AddGenericSiteCrawler(this IServiceCollection serviceDescriptors)
    {
        serviceDescriptors.AddHttpClient();
        serviceDescriptors.AddScoped<ISiteCrawler, SiteCrawler>();
    }
}
